using AutoMapper;
using Dominio;

namespace Aplicacion.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDTO>()
                .ForMember(dto => dto.Instructores, options => options.MapFrom(c => c.InstructoresLink.Select(ci => ci.instructor).ToList()))
                .ForMember(dto => dto.Precio, options => options.MapFrom(c => c.PrecioPromocion))
                .ForMember(dto => dto.Comentarios, options => options.MapFrom(c => c.ComentarioLista));
            CreateMap<CursoInstructor, CursoInstructorDTO>();
            CreateMap<Instructor, InstructorDTO>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<Precio, PrecioDTO>();
        }
    }
}
