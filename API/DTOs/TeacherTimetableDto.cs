namespace API.DTOs
{
    public class TeacherTimetableDto
    {
        public int Id { get; set; }
        public string Room { get; set; }
        public string Dayinweek { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
