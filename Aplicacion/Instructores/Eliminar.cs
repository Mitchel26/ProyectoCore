using MediatR;
using Persistencia.DapperConexion.InstructorConfiguracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }

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
                var resultado = await instructorRepository.Eliminar(request.InstructorId);
                if (resultado == 0)
                {
                    throw new Exception("No se elimino el registro");
                }
                return Unit.Value;
            }
        }
    }
}
