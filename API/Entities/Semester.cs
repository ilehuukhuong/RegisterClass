using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Semester
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
    }
}
