using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.InstructorConfiguracion;
using System.Net;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class Lista : IRequest<List<InstructorModel>> { }
        public class Manejador : IRequestHandler<Lista, List<InstructorModel>>
        {
            private readonly IInstructor instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                this.instructorRepository = instructorRepository;
            }
            public async Task<List<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                var instructores = await instructorRepository.ObtenerLista();
                if (instructores is null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "Lista de instructores vacía" });
                }
                return instructores;
            }
        }
    }
}
