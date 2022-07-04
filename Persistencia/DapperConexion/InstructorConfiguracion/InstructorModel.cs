namespace Persistencia.DapperConexion.InstructorConfiguracion
{
    public class InstructorModel
    {
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Grado { get; set; }
        public DateTime? FechaCreacion { get; set; }

    }
}
