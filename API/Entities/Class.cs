using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfStudents { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoId { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; } = true;
        public int DepartmentFacultyId { get; set; }
        [JsonIgnore]
        public DepartmentFaculty DepartmentFaculty { get; set; }
        public int SemesterId { get; set; }
        [JsonIgnore]
        public Semester Semester { get; set; }
        public List<StudentClass> StudentClasses { get; set; }
    }
}
