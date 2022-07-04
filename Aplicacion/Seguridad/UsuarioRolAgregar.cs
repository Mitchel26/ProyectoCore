using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolAgregar
    {
        public class Ejecuta : IRequest
        {
            public string Username { get; set; }
            public string RolNombre { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly RoleManager<IdentityRole> roleManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var userName = await userManager.FindByNameAsync(request.Username);
                if (userName is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "No existe el username" });
                }

                var role = await roleManager.FindByNameAsync(request.RolNombre);
                if (role is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "No existe el rol" });
                }

                var resultado = await userManager.AddToRoleAsync(userName, request.RolNombre);
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se asigno el rol al usuario");
            }
        }
    }
}
