using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public double Teachersalary { get; set; }
        public double Allowance { get; set; }
        public string Notes { get; set; }
        public bool Final { get; set; } = false;
    }
}
