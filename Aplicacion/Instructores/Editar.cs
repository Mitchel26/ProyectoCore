using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.InstructorConfiguracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Grado { get; set; }
        }

        public class EjecutaValidation : AbstractValidator<Ejecuta>
        {
            public EjecutaValidation()
            {
                RuleFor(i => i.Nombre).NotEmpty();
                RuleFor(i => i.Apellido).NotEmpty();
                RuleFor(i => i.Grado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructorRepository.Actualizar(request.InstructorId, request.Nombre, request.Apellido, request.Grado);
                if (resultado == 0)
                {
                    throw new Exception("No se actualizo el registro");
                }
                return Unit.Value;
            }
        }
    }
}
