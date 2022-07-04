using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.InstructorConfiguracion;

namespace WebAPI.Controllers
{
    public class InstructorController : MiControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> Get()
        {
            return await mediator.Send(new Consulta.Lista { });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> GetPorId(Guid id)
        {
            return await mediator.Send(new ConsultaId.InstructorUnico { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(Nuevo.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Put(Guid id, Editar.Ejecuta parametros)
        {
            parametros.InstructorId = id;
            return await mediator.Send(parametros);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await mediator.Send(new Eliminar.Ejecuta { InstructorId = id });
        }
    }
}
