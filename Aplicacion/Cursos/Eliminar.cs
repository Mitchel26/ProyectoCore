using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }

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
                // Eliminado Instructores
                var instructorDB = await context.CursoInstructor.Where(ci => ci.CursoId == request.Id).ToListAsync();
                foreach (var instructor in instructorDB)
                {
                    context.CursoInstructor.Remove(instructor);
                }

                // Eliminado Comentarios
                var comentarioDB = await context.Comentario.Where(c => c.CursoId == request.Id).ToListAsync();
                foreach (var comentario in comentarioDB)
                {
                    context.Comentario.Remove(comentario);
                }

                // Eliminado Precio
                var precio = await context.Precio.FirstOrDefaultAsync(p => p.CursoId == request.Id);
                if (precio != null)
                {
                    context.Precio.Remove(precio);
                }

                var curso = await context.Curso.FindAsync(request.Id);
                if (curso is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No de encontro el curso" });
                }
                context.Remove(curso);
                var resultado = await context.SaveChangesAsync();

                if (resultado == 0)
                {
                    throw new Exception("No se pudo eliminar el curso");
                }

                return Unit.Value;
            }
        }
    }
}
