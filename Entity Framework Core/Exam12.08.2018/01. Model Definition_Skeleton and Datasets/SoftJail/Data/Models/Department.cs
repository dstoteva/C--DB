﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }
        public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();
        public ICollection<Officer> Officers { get; set; } = new HashSet<Officer>();
    }
}
