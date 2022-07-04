using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Aplicacion.Seguridad
{
    public class RolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>>
        {
            public string UserName { get; set; }

        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> userManager;

            public Manejador(UserManager<Usuario> userManager)
            {
                this.userManager = userManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(request.UserName);
                if (user is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "No existe el usuario" });
                }

                var resultado = await userManager.GetRolesAsync(user);
                return resultado.ToList();
            }
        }
    }
}
