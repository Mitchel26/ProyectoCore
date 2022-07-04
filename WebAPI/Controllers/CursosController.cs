using Aplicacion.Cursos;
using Aplicacion.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI.Controllers
{
    public class CursosController : MiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<CursoDTO>>> Get()
        {
            return await mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetById(Guid id)
        {
            return await mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await mediator.Send(new Eliminar.Ejecuta { Id = id });
        }

        [HttpPost("paginacion")]
        public async Task<ActionResult<PaginacionModel>> Paginacion(PaginacionCurso.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }
    }
}
