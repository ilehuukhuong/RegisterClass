using API.Entities;

namespace API.DTOs
{
    public class CreateCourseGradeCategory
    {
        public int CourseId { get; set; }
        public int GradeCategoryId { get; set; }
        public int GradeItem { get; set; }
        public int GradeItemCompulsory { get; set; }
    }
}
