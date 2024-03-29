﻿using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecuta : IRequest<UsuarioData> { }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IUsuarioSesion usuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
                this.usuarioSesion = usuarioSesion;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());

                var rolesDB = await userManager.GetRolesAsync(usuario);
                var listaRoles = rolesDB.ToList();

                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Username = usuario.UserName,
                    Token = jwtGenerador.CrearToken(usuario, listaRoles)
            };
        }
    }
}
}
