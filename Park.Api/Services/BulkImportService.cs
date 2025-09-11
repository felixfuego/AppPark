using FluentValidation;
using Park.Api.Services.Interfaces;
using Park.Api.Validators;
using Park.Comun.DTOs;
using Park.Comun.Models;
using System.Text;

namespace Park.Api.Services
{
    /// <summary>
    /// Servicio para importación masiva de datos
    /// </summary>
    public class BulkImportService : IBulkImportService
    {
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IRoleService _roleService;
        private readonly BulkUserValidator _userValidator;
        private readonly BulkCompanyValidator _companyValidator;
        private readonly ILogger<BulkImportService> _logger;

        public BulkImportService(
            IFileProcessingService fileProcessingService,
            IUserService userService,
            ICompanyService companyService,
            IRoleService roleService,
            ILogger<BulkImportService> logger)
        {
            _fileProcessingService = fileProcessingService;
            _userService = userService;
            _companyService = companyService;
            _roleService = roleService;
            _userValidator = new BulkUserValidator();
            _companyValidator = new BulkCompanyValidator();
            _logger = logger;
        }

        /// <summary>
        /// Valida un archivo antes de importar
        /// </summary>
        public async Task<ImportValidationResultDto> ValidateFileAsync(BulkImportRequestWithFileDto request)
        {
            try
            {
                var result = new ImportValidationResultDto();
                var errors = new List<ImportErrorDto>();

                // Validar archivo
                var allowedExtensions = new List<string> { ".csv", ".xlsx" };
                var isValidFile = await _fileProcessingService.ValidateFileAsync(request.FileName, request.FileContent, request.FileExtension, allowedExtensions, 10);

                if (!isValidFile)
                {
                    result.IsValid = false;
                    result.ValidationErrors.Add(new ImportErrorDto
                    {
                        RowNumber = 0,
                        Field = "Archivo",
                        Value = request.FileName,
                        ErrorMessage = "El archivo no es válido. Debe ser CSV o Excel y no exceder 10MB"
                    });
                    return result;
                }

                // Procesar archivo
                List<Dictionary<string, string>> rawData;
                if (Path.GetExtension(request.FileName).ToLowerInvariant() == ".csv")
                {
                    rawData = await _fileProcessingService.ProcessCsvFileAsync(request.FileContent, request.SkipFirstRow, request.Delimiter ?? ",");
                }
                else
                {
                    rawData = await _fileProcessingService.ProcessExcelFileAsync(request.FileContent, request.SkipFirstRow);
                }

                if (!rawData.Any())
                {
                    result.IsValid = false;
                    result.ValidationErrors.Add(new ImportErrorDto
                    {
                        RowNumber = 0,
                        Field = "Datos",
                        Value = "Archivo vacío",
                        ErrorMessage = "El archivo no contiene datos válidos"
                    });
                    return result;
                }

                // Validar datos según el tipo de entidad
                if (request.EntityType.Equals("Users", StringComparison.OrdinalIgnoreCase))
                {
                    var users = _fileProcessingService.ConvertToBulkUsers(rawData);
                    result.PreviewData.AddRange(users.Take(5).Cast<object>()); // Primeras 5 filas para preview

                    for (int i = 0; i < users.Count; i++)
                    {
                        var validationResult = await _userValidator.ValidateAsync(users[i]);
                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                errors.Add(new ImportErrorDto
                                {
                                    RowNumber = i + 1,
                                    Field = error.PropertyName,
                                    Value = GetPropertyValue(users[i], error.PropertyName),
                                    ErrorMessage = error.ErrorMessage
                                });
                            }
                        }
                    }
                }
                else if (request.EntityType.Equals("Companies", StringComparison.OrdinalIgnoreCase))
                {
                    var companies = _fileProcessingService.ConvertToBulkCompanies(rawData);
                    result.PreviewData.AddRange(companies.Take(5).Cast<object>());

                    for (int i = 0; i < companies.Count; i++)
                    {
                        var validationResult = await _companyValidator.ValidateAsync(companies[i]);
                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                errors.Add(new ImportErrorDto
                                {
                                    RowNumber = i + 1,
                                    Field = error.PropertyName,
                                    Value = GetPropertyValue(companies[i], error.PropertyName),
                                    ErrorMessage = error.ErrorMessage
                                });
                            }
                        }
                    }
                }

                result.ValidationErrors = errors;
                result.ValidRecords = rawData.Count - errors.Count;
                result.InvalidRecords = errors.Count;
                result.IsValid = !errors.Any();

                _logger.LogInformation("Validación de archivo completada. Registros válidos: {Valid}, Errores: {Errors}", 
                    result.ValidRecords, result.InvalidRecords);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando archivo para importación");
                throw;
            }
        }

        /// <summary>
        /// Importa usuarios desde un archivo
        /// </summary>
        public async Task<BulkImportResultDto> ImportUsersAsync(BulkImportRequestWithFileDto request)
        {
            var result = new BulkImportResultDto();
            var errors = new List<ImportErrorDto>();
            var successCount = 0;

            try
            {
                // Procesar archivo
                List<Dictionary<string, string>> rawData;
                if (Path.GetExtension(request.FileName).ToLowerInvariant() == ".csv")
                {
                    rawData = await _fileProcessingService.ProcessCsvFileAsync(request.FileContent, request.SkipFirstRow, request.Delimiter ?? ",");
                }
                else
                {
                    rawData = await _fileProcessingService.ProcessExcelFileAsync(request.FileContent, request.SkipFirstRow);
                }

                var users = _fileProcessingService.ConvertToBulkUsers(rawData);
                result.TotalRecords = users.Count;

                // Importar usuarios
                foreach (var user in users)
                {
                    try
                    {
                        // Validar usuario
                        var validationResult = await _userValidator.ValidateAsync(user);
                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                errors.Add(new ImportErrorDto
                                {
                                    RowNumber = users.IndexOf(user) + 1,
                                    Field = error.PropertyName,
                                    Value = GetPropertyValue(user, error.PropertyName),
                                    ErrorMessage = error.ErrorMessage
                                });
                            }
                            continue;
                        }

                        // Crear usuario
                        var createUserDto = new RegisterDto
                        {
                            Username = user.Username,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Password = GenerateTemporaryPassword(),
                            ConfirmPassword = GenerateTemporaryPassword(),
                            RoleIds = new List<int>()
                        };

                        var createdUser = await _userService.CreateUserAsync(createUserDto);
                        if (createdUser != null)
                        {
                            successCount++;
                            
                            // Asignar roles si se especificaron
                            if (!string.IsNullOrEmpty(user.Roles))
                            {
                                await AssignRolesToUser(createdUser.Id, user.Roles);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = users.IndexOf(user) + 1,
                            Field = "General",
                            Value = $"{user.Username} - {user.Email}",
                            ErrorMessage = ex.Message
                        });
                    }
                }

                result.Success = true;
                result.SuccessfullyImported = successCount;
                result.FailedRecords = errors.Count;
                result.Errors = errors;
                result.Message = $"Importación completada. {successCount} usuarios importados exitosamente, {errors.Count} errores.";

                _logger.LogInformation("Importación de usuarios completada. Exitosos: {Success}, Errores: {Errors}", 
                    successCount, errors.Count);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error en la importación: {ex.Message}";
                _logger.LogError(ex, "Error en importación masiva de usuarios");
            }

            return result;
        }

        /// <summary>
        /// Importa empresas desde un archivo
        /// </summary>
        public async Task<BulkImportResultDto> ImportCompaniesAsync(BulkImportRequestWithFileDto request)
        {
            var result = new BulkImportResultDto();
            var errors = new List<ImportErrorDto>();
            var successCount = 0;

            try
            {
                // Procesar archivo
                List<Dictionary<string, string>> rawData;
                if (Path.GetExtension(request.FileName).ToLowerInvariant() == ".csv")
                {
                    rawData = await _fileProcessingService.ProcessCsvFileAsync(request.FileContent, request.SkipFirstRow, request.Delimiter ?? ",");
                }
                else
                {
                    rawData = await _fileProcessingService.ProcessExcelFileAsync(request.FileContent, request.SkipFirstRow);
                }

                var companies = _fileProcessingService.ConvertToBulkCompanies(rawData);
                result.TotalRecords = companies.Count;

                // Importar empresas
                foreach (var company in companies)
                {
                    try
                    {
                        // Validar empresa
                        var validationResult = await _companyValidator.ValidateAsync(company);
                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                errors.Add(new ImportErrorDto
                                {
                                    RowNumber = companies.IndexOf(company) + 1,
                                    Field = error.PropertyName,
                                    Value = GetPropertyValue(company, error.PropertyName),
                                    ErrorMessage = error.ErrorMessage
                                });
                            }
                            continue;
                        }

                        // Crear empresa
                        var createCompanyDto = new CreateCompanyDto
                        {
                            Name = company.Name,
                            Description = company.Description,
                            IdSitio = 1, // Valor por defecto temporal
                            ZonaIds = new List<int>() // Lista vacía por defecto
                        };

                        var createdCompany = await _companyService.CreateCompanyAsync(createCompanyDto);
                        if (createdCompany != null)
                        {
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = companies.IndexOf(company) + 1,
                            Field = "General",
                            Value = company.Name,
                            ErrorMessage = ex.Message
                        });
                    }
                }

                result.Success = true;
                result.SuccessfullyImported = successCount;
                result.FailedRecords = errors.Count;
                result.Errors = errors;
                result.Message = $"Importación completada. {successCount} empresas importadas exitosamente, {errors.Count} errores.";

                _logger.LogInformation("Importación de empresas completada. Exitosas: {Success}, Errores: {Errors}", 
                    successCount, errors.Count);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error en la importación: {ex.Message}";
                _logger.LogError(ex, "Error en importación masiva de empresas");
            }

            return result;
        }

        /// <summary>
        /// Obtiene la configuración de importación
        /// </summary>
        public async Task<ImportConfigurationDto> GetImportConfigurationAsync()
        {
            return new ImportConfigurationDto
            {
                MaxFileSizeMB = 10,
                MaxRecordsPerImport = 1000,
                AllowedFileTypes = new List<string> { ".csv", ".xlsx" },
                AllowDuplicates = false,
                SendEmailNotification = true
            };
        }

        /// <summary>
        /// Descarga una plantilla para importación
        /// </summary>
        public async Task<byte[]> DownloadTemplateAsync(string entityType, string fileFormat)
        {
            try
            {
                if (fileFormat.ToLowerInvariant() == ".csv")
                {
                    return await GenerateCsvTemplateAsync(entityType);
                }
                else
                {
                    return await GenerateExcelTemplateAsync(entityType);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando plantilla para {EntityType}", entityType);
                throw;
            }
        }

        #region Métodos Privados

        private string GetPropertyValue(object obj, string propertyName)
        {
            try
            {
                var property = obj.GetType().GetProperty(propertyName);
                return property?.GetValue(obj)?.ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task AssignRolesToUser(int userId, string rolesString)
        {
            try
            {
                var roleNames = rolesString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .ToList();

                foreach (var roleName in roleNames)
                {
                    var role = await _roleService.GetRoleByNameAsync(roleName);
                    if (role != null)
                    {
                        // Aquí implementarías la lógica para asignar el rol al usuario
                        // Dependiendo de cómo esté implementado tu sistema de asignación de roles
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error asignando roles al usuario {UserId}", userId);
            }
        }

        private string GenerateTemporaryPassword()
        {
            // Generar contraseña temporal que el usuario deberá cambiar
            return "TempPass123!";
        }

        private async Task<byte[]> GenerateCsvTemplateAsync(string entityType)
        {
            var csv = new StringBuilder();
            
            if (entityType.Equals("Users", StringComparison.OrdinalIgnoreCase))
            {
                csv.AppendLine("Username,Email,FirstName,LastName,Roles,Company,Phone");
                csv.AppendLine("john.doe,john@empresa.com,John,Doe,Admin,Empresa A,123-456-789");
                csv.AppendLine("jane.smith,jane@empresa.com,Jane,Smith,Operacion,Empresa B,987-654-321");
            }
            else if (entityType.Equals("Companies", StringComparison.OrdinalIgnoreCase))
            {
                csv.AppendLine("Name,Description,Address,Phone,Email,Website");
                csv.AppendLine("Empresa A,Descripción de la empresa A,Dirección A,123-456-789,contacto@empresaa.com,https://empresaa.com");
                csv.AppendLine("Empresa B,Descripción de la empresa B,Dirección B,987-654-321,contacto@empresab.com,https://empresab.com");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        private async Task<byte[]> GenerateExcelTemplateAsync(string entityType)
        {
            // Implementar generación de plantilla Excel usando EPPlus
            // Por ahora retornamos un CSV como placeholder
            return await GenerateCsvTemplateAsync(entityType);
        }

        #endregion
    }
}
