using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid ComentarioId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineDbContext context;

            public Manejador(CursosOnlineDbContext context)
            {
                this.context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await context.Comentario.FindAsync(request.ComentarioId);
                if (instructor is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el Id de comentario" });
                }
                context.Remove(instructor);
                var resultado = await context.SaveChangesAsync();

                if (resultado == 0)
                {
                    throw new Exception("No se elimino el instructor");
                }

                return Unit.Value;
            }
        }
    }
}
