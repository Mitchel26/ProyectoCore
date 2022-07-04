using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Persistencia;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.InstructorConfiguracion;
using Persistencia.DapperConexion.Paginacion;
using Seguridad.TokenSeguridad;
using System.Text;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString1 = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CursosOnlineDbContext>(options =>
{
    options.UseSqlServer(connectionString1);
});


// Configurando la clase ConexionConfiguracion para el uso de Dapper
builder.Services.AddOptions();
var connectionString2 = builder.Configuration.GetSection("ConnectionStrings");
builder.Services.Configure<ConexionConfiguracion>(connectionString2);


builder.Services.AddControllers(options =>
{
    // Agregando seguridad a todos los controller excepto los [AllowAnonymous]
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>()); // Configuración de FluentValidation

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(c => c.FullName); // Escribe toda la ruta del enpoint para evitar duplicados
});

// Configuración de MediatR
builder.Services.AddMediatR(typeof(Consulta.Manejador).Assembly);

// Configuración del IdentityUser(Usuario) y IdentityRole(Uso de roles y claims)
var builders = builder.Services.AddIdentityCore<Usuario>();
var identityBuilder = new IdentityBuilder(builders.UserType, builders.Services);
identityBuilder.AddRoles<IdentityRole>();
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<Usuario, IdentityRole>>();
identityBuilder.AddEntityFrameworkStores<CursosOnlineDbContext>();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();


builder.Services.TryAddSingleton<ISystemClock, SystemClock>(); // Permite agregar la hh:mm:ss cuando se inserte un registro el las entidades de seguridad
builder.Services.AddTransient<IJwtGenerador, JwtGenerador>(); // Inyectando el servicio de IJwtGenerador
builder.Services.AddTransient<IUsuarioSesion, UsuarioSesion>(); // Inyectando el servicio de Obtener el usuario en sesión

builders.Services.AddTransient<IFactoryConnection, FactoryConnection>(); // Inyectando el servicio de Conexion Dapper
builder.Services.AddScoped<IInstructor, InstructorRepository>();  // Inyectando el servicio de IIntructor
builder.Services.AddScoped<IPaginacion, PaginacionRepository>(); // Inyectando el servicio de Paginación

builder.Services.AddAutoMapper(typeof(Consulta)); // Configuración AutoMapper

// Authentication JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta")),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});


var app = builder.Build();

// Aplicar las migraciones de manera automática(Cuando se ejecuta la solución)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerfactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        // Migración Automática del MarketDbContext
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var contexto = services.GetRequiredService<CursosOnlineDbContext>();
        await contexto.Database.MigrateAsync();
        await DataPrueba.InsertarData(contexto, userManager);
    }
    catch (Exception e)
    {
        var logger = loggerfactory.CreateLogger<Program>();
        logger.LogError(e, "Errores en el proceso de migración");
    }
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ManejadorErrorMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
