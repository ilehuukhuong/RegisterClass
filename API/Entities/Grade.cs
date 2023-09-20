using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Grade
    {
        [JsonIgnore]
        public Course Course { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public GradeCategory GradeCategory { get; set; }
        public int GradeCategoryId { get; set; }
        public int Id { get; set; }
        public double Value { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }
        public int ClassId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}
