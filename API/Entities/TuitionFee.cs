using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Entities
{
    public class TuitionFee
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }
        public int AppUserId { get; set; }
        [JsonIgnore]
        public AppUser AppUser { get; set; }
        [Required]
        public string TuitionFeeType { get; set; }
        [Required]
        public double TuitionFeeAmount { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public string Notes { get; set; }
        [JsonIgnore]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
