using Dapper;
using System.Data;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepository : IPaginacion
    {
        private readonly IFactoryConnection factoryConnection;

        public PaginacionRepository(IFactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }
        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos,
                                                    IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new PaginacionModel();
            List<IDictionary<string, object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try
            {
                DynamicParameters parametros = new DynamicParameters();

                // Filtro dinámico
                foreach (var param in parametrosFiltro)
                {
                    parametros.Add("@" + param.Key, param.Value);
                }

                // Parametros de entrada
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);
                
                // Parametros de salida
                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var connection = factoryConnection.GetConnection();
                var consultaSql = await connection.QueryAsync(storeProcedure, parametros, commandType: CommandType.StoredProcedure);
                listaReporte = consultaSql.Select(x => (IDictionary<string, object>)x).ToList();
                paginacionModel.ListaRecords = listaReporte;

                // Captura los valores de los parametros de salida
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");
            }
            catch (Exception e)
            {
                throw new Exception("No se proceso los datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return paginacionModel;
        }
    }
}
