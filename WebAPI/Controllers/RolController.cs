using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class RolController : MiControllerBase
    {
        [HttpGet("rolesUsuario")]
        public async Task<ActionResult<List<string>>> RolesPorUsuario(RolesPorUsuario.Ejecuta parametro)
        {
            return await mediator.Send(parametro);
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta parametro)
        {
            return await mediator.Send(parametro);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(RolEliminar.Ejecuta parametro)
        {
            return await mediator.Send(parametro);
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> Listar()
        {
            return await mediator.Send(new RolLista.Ejecuta { });
        }

        [HttpPost("agregarRol")]
        public async Task<ActionResult<Unit>> AgregarRol(UsuarioRolAgregar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpDelete("eliminarRol")]
        public async Task<ActionResult<Unit>> EliminarRol(UsuarioRolEliminar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }
    }
}
