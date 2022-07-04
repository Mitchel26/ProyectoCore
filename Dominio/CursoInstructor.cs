namespace Dominio
{
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }
        public Guid InstructorId { get; set; }
        public Instructor instructor { get; set; }


    }
}
