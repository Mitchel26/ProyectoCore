using Dapper;
using System.Data;

namespace Persistencia.DapperConexion.InstructorConfiguracion
{
    public class InstructorRepository : IInstructor
    {
        private readonly IFactoryConnection factoryConnection;

        public InstructorRepository(IFactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }

        public async Task<List<InstructorModel>> ObtenerLista()
        {
            var instructorList = new List<InstructorModel>();
            try
            {
                var connection = factoryConnection.GetConnection();
                var consultaSql = await connection.QueryAsync<InstructorModel>("Obtener_Instructores", null,
                                                                                commandType: CommandType.StoredProcedure);
                instructorList = consultaSql.ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }

            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var instructor = new InstructorModel();
            try
            {
                var connection = factoryConnection.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>("Obtener_Instructor_Id",
                                                                            new { InstructorId = id },
                                                                            commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }

            return instructor;
        }

        public async Task<int> Nuevo(string nombre, string apellido, string grado)
        {

            int consultaSql;

            try
            {
                var connection = factoryConnection.GetConnection();
                consultaSql = await connection.ExecuteAsync("Insertar_Instructor",
                                                                new
                                                                {
                                                                    InstructorId = Guid.NewGuid(),
                                                                    Nombre = nombre,
                                                                    Apellido = apellido,
                                                                    Grado = grado
                                                                },
                                                                commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error en el registro de datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }

            return consultaSql;
        }

        public async Task<int> Actualizar(Guid instructorId, string nombre, string apellido, string grado)
        {
            int consultaSql;
            try
            {
                var connection = factoryConnection.GetConnection();
                consultaSql = await connection.ExecuteAsync("Actualizar_Instructor",
                                                            new
                                                            {
                                                                InstructorId = instructorId,
                                                                Nombre = nombre,
                                                                Apellido = apellido,
                                                                Grado = grado
                                                            },
                                                                commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("No se actualizo los datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return consultaSql;
        }

        public async Task<int> Eliminar(Guid id)
        {
            int consultaSql;
            try
            {
                var connection = factoryConnection.GetConnection();
                consultaSql = await connection.ExecuteAsync("Eliminar_Instructor", new { InstructorId = id }, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception("No se elimino los datos", e);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return consultaSql;
        }

    }
}
