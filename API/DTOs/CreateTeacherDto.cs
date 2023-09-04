using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateTeacherDto
    {
        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,}$", ErrorMessage = "Invalid password format")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter your date of birth")]
        public DateTime DateOfBirth { get; set; }
        [Required] public int GenderId { get; set; }
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter Main Subject")]
        public string MainSubject { get; set; }
        public string Concurrent { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public IFormFile File { get; set; }
    }
}
