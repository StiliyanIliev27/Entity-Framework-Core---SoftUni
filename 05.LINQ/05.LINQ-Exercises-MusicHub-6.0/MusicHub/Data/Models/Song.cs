using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicHub.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            Performers = new HashSet<Performer>();
        }
        [Key]
        public int Id { get; set; }
      
        [MaxLength(ValidationConstants.SongNameMaxLength)]
        public string Name { get; set; } = null!;
        public TimeSpan Duration { get; set; } //TimeSpan is required by default.
        public DateTime CreatedOn { get; set; }

        [Required]
        public SongGenres Genre { get; set; }
        public int? AlbumId { get; set; }        
        [ForeignKey(nameof(AlbumId))]
        public Album? Album { get; set; }
        public int? WriterId { get; set; }
        [ForeignKey(nameof(WriterId))]
        public Writer? Writer { get; set; }  
        public decimal Price { get; set; }
        public ICollection<Performer> Performers { get; set; }
    }
}
