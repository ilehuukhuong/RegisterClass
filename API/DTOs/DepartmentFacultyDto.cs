using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class DepartmentFacultyDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
