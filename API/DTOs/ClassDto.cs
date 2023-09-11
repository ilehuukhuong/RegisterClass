namespace API.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NumberOfStudents { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
        public string DepartmentFaculty { get; set; }
        public string Semester { get; set; }
    }
}
