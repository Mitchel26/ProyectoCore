using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class RolLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>> { }

        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly CursosOnlineDbContext context;

            public Manejador(CursosOnlineDbContext context)
            {
                this.context = context;
            }
            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                return await context.Roles.ToListAsync();
            }
        }
    }
}
