using System.Net;

namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion: Exception
    {
        public ManejadorExcepcion(HttpStatusCode code, object errores = null)
        {
            Code = code;
            Errores = errores;
        }

        public HttpStatusCode Code { get; }
        public object Errores { get; }
    }
}
