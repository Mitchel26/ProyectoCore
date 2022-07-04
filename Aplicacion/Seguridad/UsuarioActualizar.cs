using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(u => u.Nombre).NotEmpty();
                RuleFor(u => u.Apellido).NotEmpty();
                RuleFor(u => u.Email).NotEmpty();
                RuleFor(u => u.Username).NotEmpty();
                RuleFor(u => u.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineDbContext context;
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IPasswordHasher<Usuario> passwordHasher;

            public Manejador(CursosOnlineDbContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
                this.passwordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "El usuario no existe" });
                }

                // Verificar que el email ingresado no exista en la BD
                var existeEmail = await context.Users.AnyAsync(u => u.Email == request.Email && u.UserName != request.Username);
                if (existeEmail)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "Ya existe un usuario con este email" });
                }

                user.NombreCompleto = request.Nombre + " " + request.Apellido;
                user.Email = request.Email;
                user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

                var resultado = await userManager.UpdateAsync(user);
                if (!resultado.Succeeded)
                {
                    throw new Exception("No se actualizo los datos del usuario");
                }

                var rolesDB = await userManager.GetRolesAsync(user);
                var listaRoles = rolesDB.ToList();

                return new UsuarioData()
                {
                    NombreCompleto = user.NombreCompleto,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = jwtGenerador.CrearToken(user, listaRoles)
                };
            }
        }
    }
}
