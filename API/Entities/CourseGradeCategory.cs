namespace API.Entities
{
    public class CourseGradeCategory
    {
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public GradeCategory GradeCategory { get; set; }
        public int GradeCategoryId { get; set; }
        public int GradeItem { get; set; }
        public int GradeItemCompulsory { get; set; }
    }
}
