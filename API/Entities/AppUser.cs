using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string Address { get; set; }
        public string ParentName { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoId { get; set; }
        public string TaxCode { get; set; }
        public string MainSubject { get; set; }
        public string Concurrent { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public List<StudentClass> StudentClasses { get; set; }
    }
}
