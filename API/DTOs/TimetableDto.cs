namespace API.DTOs
{
    public class TimetableDto
    {
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public string CourseName { get;}
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
