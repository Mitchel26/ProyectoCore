﻿using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }

        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> roleManager;

            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                this.roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await roleManager.FindByNameAsync(request.Nombre);
                if (role != null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { Mensaje = "Ya existe el rol" });
                }

                var resultado = await roleManager.CreateAsync(new IdentityRole(request.Nombre));
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se puede guardar el rol");
            }
        }
    }
}
