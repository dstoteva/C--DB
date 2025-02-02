﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Hall
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        public bool Is4Dx { get; set; }
        public bool Is3D { get; set; }
        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();
        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
