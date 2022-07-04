using Aplicacion.DTOs;
using Aplicacion.ManejadorError;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Net;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid Id { get; set; }

        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineDbContext context;
            private readonly IMapper mapper;

            public Manejador(CursosOnlineDbContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso.Include(c => c.InstructoresLink)
                                               .ThenInclude(ci => ci.instructor)
                                               .Include(c => c.ComentarioLista)
                                               .Include(c => c.PrecioPromocion)
                                               .FirstOrDefaultAsync(c => c.CursoId == request.Id);
                if (curso is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No de encontro el curso" });

                }

                return mapper.Map<CursoDTO>(curso);
            }
        }

    }
}
