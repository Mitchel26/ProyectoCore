namespace Persistencia.DapperConexion.InstructorConfiguracion

{
    public interface IInstructor
    {
        Task<List<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerPorId(Guid id);
        Task<int> Nuevo(string nombre, string apellido, string grado);
        Task<int> Actualizar(Guid instructorId, string nombre, string apellido, string grado);
        Task<int> Eliminar(Guid id);
    }
}
