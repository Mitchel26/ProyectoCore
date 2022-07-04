using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string ComentarioTexto { get; set; }
            public Guid CursoId { get; set; }
            public DateTime FechaCreacion { get; set; }

        }

        public class EjecutaValidation : AbstractValidator<Ejecuta>
        {
            public EjecutaValidation()
            {
                RuleFor(c => c.Alumno).NotEmpty();
                RuleFor(c => c.Puntaje).NotEmpty();
                RuleFor(c => c.ComentarioTexto).NotEmpty();
                RuleFor(c => c.CursoId).NotEmpty();
            }
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
                var curso = await context.Curso.AnyAsync(c => c.CursoId == request.CursoId);

                if (!curso)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El Id del curso no existe" });
                }

                var comentario = new Comentario()
                {
                    ComentarioId = Guid.NewGuid(),
                    CursoId = request.CursoId,
                    Alumno = request.Alumno,
                    Puntaje = request.Puntaje,
                    ComentarioTexto = request.ComentarioTexto,
                    FechaCreacion = DateTime.UtcNow
                };

                context.Add(comentario);
                var resultado = await context.SaveChangesAsync();

                if (resultado == 0)
                {
                    throw new Exception("No se registro el comentario");
                }
                return Unit.Value;
            }
        }
    }
}
