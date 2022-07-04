using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineDbContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Esta linea permite ejecutar IdentityDbContext

            // CursoInstructor contine dos una clave compuesta (CursoId - InstructorId)    
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new { ci.CursoId, ci.InstructorId });
        }

        public DbSet<Curso> Curso { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
    }
}
