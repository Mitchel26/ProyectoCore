﻿namespace Aplicacion.DTOs
{
    public class InstructorDTO
    {
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
