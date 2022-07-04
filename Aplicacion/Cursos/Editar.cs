using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? PrecioActual { get; set; }
            public decimal? Promocion { get; set; }
            public DateTime FechaCreacion { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(c => c.Titulo).NotEmpty();
                RuleFor(c => c.Descripcion).NotEmpty();
                RuleFor(c => c.FechaPublicacion).NotEmpty();
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
                var curso = await context.Curso.FindAsync(request.CursoId);
                if (curso is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No de encontro el curso" });

                }
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                curso.FechaCreacion = DateTime.UtcNow;

                // Editar el precio del curso
                var precio = await context.Precio.FirstOrDefaultAsync(p => p.CursoId == curso.CursoId);
                if (precio != null)
                {
                    precio.PrecioActual = request.PrecioActual ?? precio.PrecioActual;
                    precio.Promocion = request.Promocion ?? precio.Promocion;

                }
                // Editar Instructores
                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        var instructoresDB = await context.CursoInstructor.Where(ci => ci.CursoId == request.CursoId).ToListAsync();
                        foreach (var instructor in instructoresDB)
                        {
                            context.CursoInstructor.Remove(instructor);
                        }
                        foreach (var id in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor()
                            {
                                CursoId = request.CursoId,
                                InstructorId = id
                            };
                            context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }
                var resultado = await context.SaveChangesAsync();
                if (resultado == 0)
                {
                    throw new Exception("No se actualizo el curso");
                }
                return Unit.Value;
            }
        }
    }
}
