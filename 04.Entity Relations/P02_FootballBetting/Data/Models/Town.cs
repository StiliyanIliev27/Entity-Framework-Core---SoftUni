using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        public Town()
        {
            Players = new List<Player>();
            Teams = new List<Team>();
        }

        [Key]
        public int TownId { get; set; }

        [MaxLength(Const.Constants.CountryAndTownNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; } = null!;      
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
