namespace API.DTOs
{
    public class CourseGradeCategoryDto
    {
        public string Semester { get; set; }
        public string Course { get; set; }
        public string GradeCategory { get; set; }
        public int GradeItem { get; set; }
        public int GradeItemCompulsory { get; set; }
    }
}
