using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateCourseGradeCategory, CourseGradeCategory>();
            CreateMap<CourseGradeCategory,CourseGradeCategoryDto>()
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Course.Semester.Name))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.GradeCategory, opt => opt.MapFrom(src => src.GradeCategory.Name));
            CreateMap<CreateClassDto, Class>();
            CreateMap<UpdateClassDto, Class>();
            CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.DepartmentFaculty, opt => opt.MapFrom(src => src.DepartmentFaculty.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.Name));
            CreateMap<DepartmentFaculty, DepartmentFacultyDto>();
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.DepartmentFaculty, opt => opt.MapFrom(src => src.DepartmentFaculty.Name))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester.Name));
            CreateMap<Semester, SemesterDto>();
            CreateMap<SemesterDto, Semester>();
            CreateMap<AppUser, MemberDto>();
            CreateMap<AppUser, StudentDto>();
            CreateMap<AppUser, TeacherDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Name));
            CreateMap<RegisterDto, AppUser>();
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}
