using System.Text.Json.Serialization;

namespace API.Entities
{
    public class GradeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Coefficient { get; set; }
        [JsonIgnore]
        public List<CourseGradeCategory> CourseGradeCategories { get; set; }
        [JsonIgnore]
        public List<Grade> Grades { get; set; }
    }
}
