using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        [Range(18, 70)]
        public int Age { get; set; }
        [Range(typeof(decimal), "0.0", "1000000000000000000")]
        public decimal NetWorth { get; set; }
        public ICollection<SongPerformer> PerformerSongs { get; set; } = new HashSet<SongPerformer>();
    }
}
