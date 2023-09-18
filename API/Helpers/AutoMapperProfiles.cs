using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Timetable, TeacherTimetableDto>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class.Name));
            CreateMap<Timetable, StudentTimetableDto>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class.Name));
            CreateMap<Timetable, Timetable>();
            CreateMap<Timetable, TimetableDto>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.AppUser.FullName))
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.AppUser.UserName))      
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class.Name));
            CreateMap<UpdateTeacherDto, AppUser>();
            CreateMap<CreateTeacherDto, AppUser>();
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
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.Name))
                .ForMember(dest => dest.MainSubject, opt => opt.MapFrom(src => src.Course.Name));
            CreateMap<RegisterDto, AppUser>();
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}
