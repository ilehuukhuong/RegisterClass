namespace API.Entities
{
    public class StudentClass
    {
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public Class Class { get; set; }
        public int ClassId { get; set; }
    }
}
