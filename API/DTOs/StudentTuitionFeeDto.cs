namespace API.DTOs
{
    public class StudentTuitionFeeDto
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string TeacherName { get; set; }
        public double TuitionFeeAmount { get; set; }
    }
}
