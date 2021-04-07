using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [NotMapped]
        public decimal Price => this.Songs.Sum(x => x.Price);
        public int? ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}
