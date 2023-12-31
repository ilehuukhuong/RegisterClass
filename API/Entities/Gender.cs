﻿using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
