using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdateClassDto
    {
        public int Id { get; set; }
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
        [Required]
        public bool Status { get; set; }
        public IFormFile File { get; set; }
    }
}
