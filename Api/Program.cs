using System.Text;
using Api;
using Api.OpenApi;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Render (y otros PaaS) inyectan el puerto a escuchar via la variable de entorno PORT.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

const string CorsPolicy = "RifaFrontend";

// Add services to the container.

var allowedOriginsSetting = builder.Configuration["Cors:AllowedOrigins"]
    ?? "https://rifa-ui.vercel.app";

var allowedOrigins = allowedOriginsSetting
    .Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Select(o => o.Trim())
    .ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Autenticacion JWT. La clave puede definirse de dos formas (en este orden de prioridad):
//   1. Variable de entorno plana JWT_KEY (recomendado en Render).
//   2. Configuracion Jwt:Key (appsettings o variable Jwt__Key con doble guion bajo).
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
    ?? jwtSection["Key"]
    ?? throw new InvalidOperationException(
        "Falta la clave JWT. Defina la variable de entorno 'JWT_KEY' o la clave 'Jwt:Key' en la configuracion.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Aplica automaticamente las migraciones pendientes al arrancar (idempotente).
// Asi, si borras las tablas (incluyendo __EFMigrationsHistory), clonas el repo o
// despliegas contra una BD nueva, el esquema se recrea solo al ejecutar la app.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RifaDbContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.

// Tras el proxy TLS de Render la redireccion https no aplica y genera warnings.
if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseRouting();

// CORS debe ir DESPUES de UseRouting y ANTES de UseExceptionHandler: si el manejador
// de errores corre primero, al reescribir la respuesta (p.ej. un 401 de login) se pierden
// los headers Access-Control-Allow-* y el navegador reporta un falso "CORS error".
app.UseCors(CorsPolicy);

app.UseExceptionHandler();

app.MapOpenApi();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/openapi/v1.json", "Rifa API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();
