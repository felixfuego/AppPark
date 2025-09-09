using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;
using System.Security.Cryptography;
using System.Text;

namespace Park.Api.Services
{
    public class FileService : IFileService
    {
        private readonly ParkDbContext _context;
        private readonly ILogger<FileService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath;

        public FileService(ParkDbContext context, ILogger<FileService> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
            _uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            
            // Crear directorio de uploads si no existe
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<FileUploadResponseDto> UploadFileAsync(UploadFileDto uploadDto, int? userId = null)
        {
            try
            {
                if (uploadDto.Archivo == null || uploadDto.Archivo.Length == 0)
                {
                    return new FileUploadResponseDto
                    {
                        Exitoso = false,
                        Mensaje = "No se ha proporcionado ningún archivo",
                        Error = "Archivo vacío"
                    };
                }

                // Validar tipo de archivo
                if (!await IsValidFileTypeAsync(uploadDto.Archivo.FileName, uploadDto.Tipo))
                {
                    return new FileUploadResponseDto
                    {
                        Exitoso = false,
                        Mensaje = "Tipo de archivo no permitido",
                        Error = "Tipo de archivo inválido"
                    };
                }

                // Generar nombre único para el archivo
                var extension = await GetFileExtensionAsync(uploadDto.Archivo.FileName);
                var uniqueFileName = await GenerateUniqueFileNameAsync(uploadDto.Archivo.FileName);
                var filePath = Path.Combine(_uploadPath, uniqueFileName);

                // Crear directorio por tipo si no existe
                var typePath = Path.Combine(_uploadPath, uploadDto.Tipo.ToString());
                if (!Directory.Exists(typePath))
                {
                    Directory.CreateDirectory(typePath);
                }

                filePath = Path.Combine(typePath, uniqueFileName);

                // Guardar archivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadDto.Archivo.CopyToAsync(stream);
                }

                // Guardar información en base de datos
                var file = new FileEntity
                {
                    NombreOriginal = uploadDto.Archivo.FileName,
                    NombreArchivo = uniqueFileName,
                    RutaArchivo = filePath,
                    Extension = extension,
                    TamañoBytes = uploadDto.Archivo.Length,
                    TipoMime = await GetMimeTypeAsync(uploadDto.Archivo.FileName),
                    Tipo = uploadDto.Tipo,
                    Descripcion = uploadDto.Descripcion,
                    IdEntidad = uploadDto.IdEntidad,
                    Entidad = uploadDto.Entidad,
                    IdUsuario = userId,
                    IsActive = true,
                    FechaSubida = DateTime.UtcNow
                };

                _context.Files.Add(file);
                await _context.SaveChangesAsync();

                // Cargar relaciones
                await _context.Entry(file)
                    .Reference(f => f.Usuario)
                    .LoadAsync();

                return new FileUploadResponseDto
                {
                    Exitoso = true,
                    Mensaje = "Archivo subido exitosamente",
                    Archivo = MapToDto(file)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir archivo: {FileName}", uploadDto.Archivo?.FileName);
                return new FileUploadResponseDto
                {
                    Exitoso = false,
                    Mensaje = "Error interno del servidor",
                    Error = ex.Message
                };
            }
        }

        public async Task<FileDto?> GetFileAsync(int fileId)
        {
            try
            {
                var file = await _context.Files
                    .Include(f => f.Usuario)
                    .FirstOrDefaultAsync(f => f.Id == fileId && f.IsActive);

                return file != null ? MapToDto(file) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener archivo {FileId}", fileId);
                return null;
            }
        }

        public async Task<FileDto?> GetFileByPathAsync(string filePath)
        {
            try
            {
                var file = await _context.Files
                    .Include(f => f.Usuario)
                    .FirstOrDefaultAsync(f => f.RutaArchivo == filePath && f.IsActive);

                return file != null ? MapToDto(file) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener archivo por ruta {FilePath}", filePath);
                return null;
            }
        }

        public async Task<byte[]?> DownloadFileAsync(int fileId)
        {
            try
            {
                var file = await _context.Files
                    .FirstOrDefaultAsync(f => f.Id == fileId && f.IsActive);

                if (file == null || !await FileExistsAsync(file.RutaArchivo))
                {
                    return null;
                }

                return await File.ReadAllBytesAsync(file.RutaArchivo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al descargar archivo {FileId}", fileId);
                return null;
            }
        }

        public async Task<byte[]?> DownloadFileByPathAsync(string filePath)
        {
            try
            {
                if (!await FileExistsAsync(filePath))
                {
                    return null;
                }

                return await File.ReadAllBytesAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al descargar archivo por ruta {FilePath}", filePath);
                return null;
            }
        }

        public async Task<bool> DeleteFileAsync(int fileId)
        {
            try
            {
                var file = await _context.Files
                    .FirstOrDefaultAsync(f => f.Id == fileId && f.IsActive);

                if (file == null)
                {
                    return false;
                }

                // Eliminar archivo físico
                if (await FileExistsAsync(file.RutaArchivo))
                {
                    File.Delete(file.RutaArchivo);
                }

                // Marcar como eliminado en base de datos
                file.IsActive = false;
                file.FechaEliminacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar archivo {FileId}", fileId);
                return false;
            }
        }

        public async Task<bool> DeleteFileByPathAsync(string filePath)
        {
            try
            {
                if (!await FileExistsAsync(filePath))
                {
                    return false;
                }

                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar archivo por ruta {FilePath}", filePath);
                return false;
            }
        }

        public async Task<IEnumerable<FileDto>> GetFilesAsync(FileSearchDto searchDto)
        {
            try
            {
                var query = _context.Files
                    .Include(f => f.Usuario)
                    .Where(f => f.IsActive)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(searchDto.NombreOriginal))
                {
                    query = query.Where(f => f.NombreOriginal.Contains(searchDto.NombreOriginal));
                }

                if (!string.IsNullOrEmpty(searchDto.Extension))
                {
                    query = query.Where(f => f.Extension == searchDto.Extension);
                }

                if (searchDto.Tipo.HasValue)
                {
                    query = query.Where(f => f.Tipo == searchDto.Tipo.Value);
                }

                if (searchDto.IdUsuario.HasValue)
                {
                    query = query.Where(f => f.IdUsuario == searchDto.IdUsuario.Value);
                }

                if (searchDto.IdEntidad.HasValue)
                {
                    query = query.Where(f => f.IdEntidad == searchDto.IdEntidad.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.Entidad))
                {
                    query = query.Where(f => f.Entidad == searchDto.Entidad);
                }

                if (searchDto.FechaInicio.HasValue)
                {
                    query = query.Where(f => f.FechaSubida.Date >= searchDto.FechaInicio.Value.Date);
                }

                if (searchDto.FechaFin.HasValue)
                {
                    query = query.Where(f => f.FechaSubida.Date <= searchDto.FechaFin.Value.Date);
                }

                if (searchDto.TamañoMinimo.HasValue)
                {
                    query = query.Where(f => f.TamañoBytes >= searchDto.TamañoMinimo.Value);
                }

                if (searchDto.TamañoMaximo.HasValue)
                {
                    query = query.Where(f => f.TamañoBytes <= searchDto.TamañoMaximo.Value);
                }

                // Aplicar ordenamiento
                query = query.OrderByDescending(f => f.FechaSubida);

                // Aplicar paginación
                var files = await query
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                return files.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener archivos");
                throw;
            }
        }

        public async Task<PagedResultDto<FileDto>> SearchFilesAsync(FileSearchDto searchDto)
        {
            try
            {
                var query = _context.Files
                    .Include(f => f.Usuario)
                    .Where(f => f.IsActive)
                    .AsQueryable();

                // Aplicar filtros (mismo código que GetFilesAsync)
                if (!string.IsNullOrEmpty(searchDto.NombreOriginal))
                {
                    query = query.Where(f => f.NombreOriginal.Contains(searchDto.NombreOriginal));
                }

                if (!string.IsNullOrEmpty(searchDto.Extension))
                {
                    query = query.Where(f => f.Extension == searchDto.Extension);
                }

                if (searchDto.Tipo.HasValue)
                {
                    query = query.Where(f => f.Tipo == searchDto.Tipo.Value);
                }

                if (searchDto.IdUsuario.HasValue)
                {
                    query = query.Where(f => f.IdUsuario == searchDto.IdUsuario.Value);
                }

                if (searchDto.IdEntidad.HasValue)
                {
                    query = query.Where(f => f.IdEntidad == searchDto.IdEntidad.Value);
                }

                if (!string.IsNullOrEmpty(searchDto.Entidad))
                {
                    query = query.Where(f => f.Entidad == searchDto.Entidad);
                }

                if (searchDto.FechaInicio.HasValue)
                {
                    query = query.Where(f => f.FechaSubida.Date >= searchDto.FechaInicio.Value.Date);
                }

                if (searchDto.FechaFin.HasValue)
                {
                    query = query.Where(f => f.FechaSubida.Date <= searchDto.FechaFin.Value.Date);
                }

                if (searchDto.TamañoMinimo.HasValue)
                {
                    query = query.Where(f => f.TamañoBytes >= searchDto.TamañoMinimo.Value);
                }

                if (searchDto.TamañoMaximo.HasValue)
                {
                    query = query.Where(f => f.TamañoBytes <= searchDto.TamañoMaximo.Value);
                }

                // Obtener total de registros
                var totalCount = await query.CountAsync();

                // Aplicar ordenamiento y paginación
                var files = await query
                    .OrderByDescending(f => f.FechaSubida)
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                var filesDto = files.Select(MapToDto);

                return new PagedResultDto<FileDto>
                {
                    Data = filesDto,
                    TotalCount = totalCount,
                    Page = searchDto.Page,
                    PageSize = searchDto.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar archivos");
                throw;
            }
        }

        public async Task<FileStatsDto> GetFileStatsAsync()
        {
            try
            {
                var hoy = DateTime.Today;
                var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek);
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

                var totalArchivos = await _context.Files.CountAsync(f => f.IsActive);
                var tamañoTotal = await _context.Files.Where(f => f.IsActive).SumAsync(f => f.TamañoBytes);
                var archivosHoy = await _context.Files.CountAsync(f => f.FechaSubida.Date == hoy && f.IsActive);
                var archivosEstaSemana = await _context.Files.CountAsync(f => f.FechaSubida.Date >= inicioSemana && f.IsActive);
                var archivosEsteMes = await _context.Files.CountAsync(f => f.FechaSubida.Date >= inicioMes && f.IsActive);

                var archivosPorTipo = await _context.Files
                    .Where(f => f.IsActive)
                    .GroupBy(f => f.Tipo)
                    .Select(g => new { Tipo = g.Key.ToString(), Cantidad = g.Count() })
                    .ToDictionaryAsync(x => x.Tipo, x => x.Cantidad);

                var archivosPorExtension = await _context.Files
                    .Where(f => f.IsActive)
                    .GroupBy(f => f.Extension)
                    .Select(g => new { Extension = g.Key, Cantidad = g.Count() })
                    .ToDictionaryAsync(x => x.Extension, x => x.Cantidad);

                var tamañoPorTipo = await _context.Files
                    .Where(f => f.IsActive)
                    .GroupBy(f => f.Tipo)
                    .Select(g => new { Tipo = g.Key.ToString(), Tamaño = g.Sum(f => f.TamañoBytes) })
                    .ToDictionaryAsync(x => x.Tipo, x => x.Tamaño);

                var ultimaSubida = await _context.Files
                    .Where(f => f.IsActive)
                    .OrderByDescending(f => f.FechaSubida)
                    .Select(f => f.FechaSubida)
                    .FirstOrDefaultAsync();

                return new FileStatsDto
                {
                    TotalArchivos = totalArchivos,
                    TamañoTotalBytes = tamañoTotal,
                    ArchivosHoy = archivosHoy,
                    ArchivosEstaSemana = archivosEstaSemana,
                    ArchivosEsteMes = archivosEsteMes,
                    ArchivosPorTipo = archivosPorTipo,
                    ArchivosPorExtension = archivosPorExtension,
                    TamañoPorTipo = tamañoPorTipo,
                    UltimaSubida = ultimaSubida
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de archivos");
                throw;
            }
        }

        public async Task<FileInfoDto> GetFileInfoAsync(string filePath)
        {
            try
            {
                if (!await FileExistsAsync(filePath))
                {
                    return new FileInfoDto { Existe = false };
                }

                var fileInfo = new FileInfo(filePath);
                var extension = await GetFileExtensionAsync(fileInfo.Name);

                return new FileInfoDto
                {
                    NombreOriginal = fileInfo.Name,
                    Extension = extension,
                    TamañoBytes = fileInfo.Length,
                    TipoMime = await GetMimeTypeAsync(fileInfo.Name),
                    FechaCreacion = fileInfo.CreationTimeUtc,
                    FechaModificacion = fileInfo.LastWriteTimeUtc,
                    Existe = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener información del archivo {FilePath}", filePath);
                return new FileInfoDto { Existe = false };
            }
        }

        public async Task<bool> FileExistsAsync(string filePath)
        {
            return await Task.FromResult(File.Exists(filePath));
        }

        public async Task<string> GenerateUniqueFileNameAsync(string originalFileName)
        {
            var extension = await GetFileExtensionAsync(originalFileName);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = Guid.NewGuid().ToString("N")[..8];
            
            return $"{nameWithoutExtension}_{timestamp}_{random}{extension}";
        }

        public async Task<string> GetFileExtensionAsync(string fileName)
        {
            return await Task.FromResult(Path.GetExtension(fileName).ToLowerInvariant());
        }

        public async Task<string> GetMimeTypeAsync(string fileName)
        {
            var extension = await GetFileExtensionAsync(fileName);
            
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".txt" => "text/plain",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }

        public async Task<bool> IsValidFileTypeAsync(string fileName, TipoArchivo tipo)
        {
            var extension = await GetFileExtensionAsync(fileName);
            
            if (!ExtensionesPermitidas.PorTipo.TryGetValue(tipo, out var allowedExtensions))
            {
                return false;
            }

            return allowedExtensions.Contains(extension);
        }

        public async Task<long> GetFileSizeAsync(string filePath)
        {
            try
            {
                if (!await FileExistsAsync(filePath))
                {
                    return 0;
                }

                var fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tamaño del archivo {FilePath}", filePath);
                return 0;
            }
        }

        public async Task<bool> CleanTempFilesAsync()
        {
            try
            {
                var tempPath = Path.Combine(_uploadPath, "temp");
                if (!Directory.Exists(tempPath))
                {
                    return true;
                }

                var tempFiles = Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories);
                var deletedCount = 0;

                foreach (var file in tempFiles)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTimeUtc < DateTime.UtcNow.AddHours(-24))
                    {
                        File.Delete(file);
                        deletedCount++;
                    }
                }

                _logger.LogInformation("Eliminados {Count} archivos temporales", deletedCount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar archivos temporales");
                return false;
            }
        }

        public async Task<bool> CleanOldFilesAsync(int diasRetencion = 30)
        {
            try
            {
                var fechaLimite = DateTime.UtcNow.AddDays(-diasRetencion);
                var archivosAntiguos = await _context.Files
                    .Where(f => f.FechaSubida < fechaLimite && f.IsActive)
                    .ToListAsync();

                var deletedCount = 0;
                foreach (var archivo in archivosAntiguos)
                {
                    if (await FileExistsAsync(archivo.RutaArchivo))
                    {
                        File.Delete(archivo.RutaArchivo);
                    }

                    archivo.IsActive = false;
                    archivo.FechaEliminacion = DateTime.UtcNow;
                    deletedCount++;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Eliminados {Count} archivos antiguos", deletedCount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar archivos antiguos");
                return false;
            }
        }

        private static FileDto MapToDto(FileEntity file)
        {
            return new FileDto
            {
                Id = file.Id,
                NombreOriginal = file.NombreOriginal,
                NombreArchivo = file.NombreArchivo,
                RutaArchivo = file.RutaArchivo,
                Extension = file.Extension,
                TamañoBytes = file.TamañoBytes,
                TipoMime = file.TipoMime,
                Tipo = file.Tipo,
                Descripcion = file.Descripcion,
                IdEntidad = file.IdEntidad,
                Entidad = file.Entidad,
                IdUsuario = file.IdUsuario,
                IsActive = file.IsActive,
                FechaSubida = file.FechaSubida,
                FechaEliminacion = file.FechaEliminacion,
                Usuario = file.Usuario != null ? new UserDto
                {
                    Id = file.Usuario.Id,
                    Username = file.Usuario.Username,
                    Email = file.Usuario.Email,
                    FirstName = file.Usuario.FirstName,
                    LastName = file.Usuario.LastName,
                    IsActive = file.Usuario.IsActive
                } : null
            };
        }
    }
}
