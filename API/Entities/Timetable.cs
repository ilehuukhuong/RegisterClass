using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Timetable
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }
        public int ClassId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public string Room { get; set; }
        public string Dayinweek { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
    }
}
