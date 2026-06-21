using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    /// <summary>Variable de entorno que, si esta definida, tiene prioridad sobre appsettings.</summary>
    public const string ConnectionStringEnvVar = "RIFA_DB_CONNECTION";

    /// <summary>Variable de entorno plana para la clave JWT; tiene prioridad sobre 'Jwt:Key'.</summary>
    public const string JwtKeyEnvVar = "JWT_KEY";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Prioridad: variable de entorno RIFA_DB_CONNECTION > ConnectionStrings:RifaDb (appsettings o ConnectionStrings__RifaDb).
        var connectionString =
            Environment.GetEnvironmentVariable(ConnectionStringEnvVar)
            ?? configuration.GetConnectionString("RifaDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"No se encontro la cadena de conexion. Defina la variable de entorno '{ConnectionStringEnvVar}' " +
                "o la clave 'ConnectionStrings:RifaDb' en la configuracion.");
        }

        services.AddDbContext<RifaDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        // Si existe la variable de entorno plana JWT_KEY, tiene prioridad sobre 'Jwt:Key'
        // para que la clave de firma sea la misma que valida la Api en Render.
        var jwtKey = Environment.GetEnvironmentVariable(JwtKeyEnvVar);
        if (!string.IsNullOrWhiteSpace(jwtKey))
            services.PostConfigure<JwtSettings>(settings => settings.Key = jwtKey);

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ITalonarioRepository, TalonarioRepository>();
        services.AddScoped<IBoletaRepository, BoletaRepository>();

        return services;
    }
}
