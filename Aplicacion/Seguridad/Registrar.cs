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
    public class Registrar
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

            public Manejador(CursosOnlineDbContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existeEmail = await context.Users.Where(u => u.Email == request.Email).AnyAsync();
                if (existeEmail)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "Existe un usuario registrado con este email" });
                }

                var existeUsername = await context.Users.Where(u => u.UserName == request.Username).AnyAsync();
                if (existeUsername)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "Existe un usuario con este username" });
                }

                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellido,
                    Email = request.Email,
                    UserName = request.Username,
                };

                var resultado = await userManager.CreateAsync(usuario, request.Password);

                // Cuando se registra el usuario no tiene roles

                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        Username = usuario.UserName,
                        Token = jwtGenerador.CrearToken(usuario, null)
                    };
                }

                throw new Exception("Error: No se pudo crear el usuario");
            }
        }
    }
}
