﻿namespace API.DTOs
{
    public class TimetableDto
    {
        public int Id { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherName { get; set; }
        public string Class { get; set; }
        public string Course { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
