using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(u => u.Email).NotEmpty();
                RuleFor(u => u.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly SignInManager<Usuario> signInManager;
            private readonly IJwtGenerador jwtGenerador;

            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByEmailAsync(request.Email);
                if (usuario is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado = await signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                var rolesDB = await userManager.GetRolesAsync(usuario);
                var listaRoles = rolesDB.ToList();

                if (resultado.Succeeded)
                {
                    return new UsuarioData()
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        Username = usuario.UserName,
                        Token = jwtGenerador.CrearToken(usuario, listaRoles),
                        Imagen = null
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);

            }
        }
    }
}
