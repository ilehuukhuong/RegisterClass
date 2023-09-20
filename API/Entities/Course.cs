using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DepartmentFacultyId { get; set; }
        [JsonIgnore]
        public DepartmentFaculty DepartmentFaculty { get; set; }
        public int SemesterId { get; set; }
        [JsonIgnore]
        public Semester Semester { get; set; }
        [JsonIgnore]
        public List<CourseGradeCategory> CourseGradeCategories { get; set; }
        [JsonIgnore]
        public List<Timetable> TimetableClasses { get; set; }
        [JsonIgnore]
        public List<Grade> Grades { get; set; }
    }
}
