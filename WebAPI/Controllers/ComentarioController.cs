using Aplicacion.Comentarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ComentarioController : MiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Post(Nuevo.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await mediator.Send(new Eliminar.Ejecuta { ComentarioId = id });
        }
    }
}
