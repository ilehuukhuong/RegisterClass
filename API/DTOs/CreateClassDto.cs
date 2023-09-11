using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateClassDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int NumberOfStudents { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int DepartmentFacultyId { get; set; }
        [Required]
        public int SemesterId { get; set; }
        public IFormFile File { get; set; }
    }
}
