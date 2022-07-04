using Aplicacion.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDTO>> { }
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDTO>>
        {
            private readonly CursosOnlineDbContext context;
            private readonly IMapper mapper;

            public Manejador(CursosOnlineDbContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
            public async Task<List<CursoDTO>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context.Curso.Include(c => c.InstructoresLink)
                                                .ThenInclude(ci => ci.instructor)
                                                .Include(c => c.ComentarioLista)
                                                .Include(c => c.PrecioPromocion).ToListAsync();
                return mapper.Map<List<CursoDTO>>(cursos);
            }
        }
    }
}
