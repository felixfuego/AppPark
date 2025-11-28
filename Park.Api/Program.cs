using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Park.Api.Configuration;
using Park.Api.Data;
using Park.Api.Services;
using Park.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Park.Api.Validators;
using Park.Comun.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings?.Issuer,
            ValidAudience = jwtSettings?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key ?? string.Empty))
        };
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Agregar validación automática de modelos
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Park Management API", 
        Version = "v1",
        Description = "API para gestión de parques de diversiones con autenticación JWT",
        Contact = new OpenApiContact
        {
            Name = "Park Development Team",
            Email = "dev@park.com"
        }
    });

    // Configurar autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
    
    // Política específica para desarrollo local
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5077", "https://localhost:5077")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add DbContext
builder.Services.AddDbContext<ParkDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LiveData")));

        // Register Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        
        // Register Park Industrial Services
        builder.Services.AddScoped<ICompanyService, CompanyService>();
        
        // Register Bulk Import Services
        builder.Services.AddScoped<IBulkImportService, BulkImportService>();
        builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

        // Nuevos servicios para el modelo de datos actualizado
        builder.Services.AddScoped<ISitioService, SitioService>();
        builder.Services.AddScoped<IZonaService, ZonaService>();
        builder.Services.AddScoped<ICentroService, CentroService>();
        builder.Services.AddScoped<IColaboradorService, ColaboradorService>();
        builder.Services.AddScoped<IVisitorService, VisitorService>();
        builder.Services.AddScoped<IVisitaService, VisitaService>();
        builder.Services.AddScoped<IQrService, QrService>();
        builder.Services.AddScoped<ExcelService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IAuditService, AuditService>();
        builder.Services.AddScoped<IFileService, FileService>();

        // Register Validators
        builder.Services.AddScoped<IValidator<CreateVisitaDto>, CreateVisitaValidator>();
        builder.Services.AddScoped<IValidator<UpdateVisitaDto>, UpdateVisitaValidator>();
        builder.Services.AddScoped<IValidator<VisitaCheckInDto>, VisitaCheckInValidator>();
        builder.Services.AddScoped<IValidator<VisitaCheckOutDto>, VisitaCheckOutValidator>();
        builder.Services.AddScoped<IValidator<CreateColaboradorDto>, CreateColaboradorValidator>();
        builder.Services.AddScoped<IValidator<UpdateColaboradorDto>, UpdateColaboradorValidator>();
        builder.Services.AddScoped<IValidator<CreateCompanyDto>, CreateCompanyValidator>();
        builder.Services.AddScoped<IValidator<UpdateCompanyDto>, UpdateCompanyValidator>();
        builder.Services.AddScoped<IValidator<CreateSitioDto>, CreateSitioValidator>();
        builder.Services.AddScoped<IValidator<UpdateSitioDto>, UpdateSitioValidator>();
        builder.Services.AddScoped<IValidator<CreateZonaDto>, CreateZonaValidator>();
        builder.Services.AddScoped<IValidator<UpdateZonaDto>, UpdateZonaValidator>();
        builder.Services.AddScoped<IValidator<CreateCentroDto>, CreateCentroValidator>();
        builder.Services.AddScoped<IValidator<UpdateCentroDto>, UpdateCentroValidator>();

var app = builder.Build();

// Inicializar la base de datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ParkDbContext>();
    try
    {
        await DbInitializer.InitializeAsync(context);
        Console.WriteLine("Base de datos inicializada correctamente");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error inicializando la base de datos: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Park Management API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Park Management API Documentation";
    });
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.Run();
