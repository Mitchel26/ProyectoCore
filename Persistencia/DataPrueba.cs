using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineDbContext context, UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario()
                {
                    NombreCompleto = "Jhon Varas",
                    UserName = "jvaras",
                    Email = "jhonmitchelupn@gmail.com"
                };

                await userManager.CreateAsync(usuario, "Codigo@2022");

            }
        }
    }
}
