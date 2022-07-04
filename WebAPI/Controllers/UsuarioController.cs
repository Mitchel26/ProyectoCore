using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous] // Controller sin seguridad
    public class UsuarioController : MiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> ObtenerUsuario()
        {
            return await mediator.Send(new UsuarioActual.Ejecuta { });
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }
    }
}
