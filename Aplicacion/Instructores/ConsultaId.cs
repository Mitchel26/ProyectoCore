using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.InstructorConfiguracion;
using System.Net;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class InstructorUnico : IRequest<InstructorModel>
        {
            public Guid Id { get; set; }

        }

        public class Manejador : IRequestHandler<InstructorUnico, InstructorModel>
        {
            private readonly IInstructor instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }
            public async Task<InstructorModel> Handle(InstructorUnico request, CancellationToken cancellationToken)
            {
                var instructor = await instructorRepository.ObtenerPorId(request.Id);
                if (instructor is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el instructor" });
                }
                return instructor;
            }
        }
    }
}
