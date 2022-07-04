using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal PrecioActual { get; set; }
            public decimal Promocion { get; set; }
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
                // Agregar Curso
                var cursoId = Guid.NewGuid();
                var curso = new Curso()
                {
                    CursoId = cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion = DateTime.UtcNow
                };
                context.Add(curso);

                // Agregar Instructores
                if (request.ListaInstructor != null)
                {
                    var instructoresIds = await context.Instructor.Where(i => request.ListaInstructor.Contains(i.InstructorId)).Select(x => x.InstructorId).ToListAsync();
                    if (request.ListaInstructor.Count == instructoresIds.Count)
                    {
                        foreach (var instructorId in request.ListaInstructor)
                        {
                            var cursoInstructor = new CursoInstructor
                            {
                                CursoId = cursoId,
                                InstructorId = instructorId
                            };

                            context.Add(cursoInstructor);
                        }
                    }
                    else
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "Hay un Id de instructor que no existe" });
                    }
                }

                // Agregar precio
                var precio = new Precio()
                {
                    PrecioId = Guid.NewGuid(),
                    CursoId = cursoId,
                    PrecioActual = request.PrecioActual,
                    Promocion = request.Promocion
                };

                context.Add(precio);

                var resultado = await context.SaveChangesAsync();

                if (resultado == 0)
                {
                    throw new Exception("No se pudo insertar el curso");
                }

                return Unit.Value;
            }
        }
    }
}
