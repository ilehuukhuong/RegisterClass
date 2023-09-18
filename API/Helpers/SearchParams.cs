using API.Entities;

namespace API.Helpers
{
    public class SearchParams : PaginationParams
    {
        public string Search { get; set; }
        public string Code { get; set; }
        public string CourseName { get; set; }
        public int? DepartmentFacultyId { get; set; }
        public int? SemesterId { get; set; }
    }
}
