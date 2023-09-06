using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class SemesterDto
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDay { get; set; }
        [Required]
        public DateTime EndDay { get; set; }
    }
}
