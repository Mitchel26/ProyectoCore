using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Persistencia.DapperConexion
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection connection;
        private readonly IOptions<ConexionConfiguracion> configs;

        public FactoryConnection(IOptions<ConexionConfiguracion> configs)
        {
            this.configs = configs;
        }
        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            if (connection is null)
            {
                connection = new SqlConnection(configs.Value.DefaultConnection);
            }

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
