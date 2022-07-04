using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UsuarioSesion(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        public string ObtenerUsuarioSesion()
        {
            var userName = contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}
