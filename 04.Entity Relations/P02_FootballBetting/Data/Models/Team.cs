using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [MaxLength(Const.Constants.TeamNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;
        public string? LogoUrl { get; set; }

        [MaxLength(Const.Constants.InitialsMaxLength)]
        public string Initials { get; set; } = null!;
        public decimal Budget { get; set; }
        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        public virtual Color PrimaryKitColor { get; set; } = null!;
        public int SecondaryKitColorId { get; set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        public virtual Color SecondaryKitColor { get; set; } = null!;
        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public virtual Town Town { get; set; } = null!;
        public virtual ICollection<Player> Players { get; set; }

        [InverseProperty(nameof(Game.HomeTeam))]
        public virtual ICollection<Game> HomeGames { get; set; }
       
        [InverseProperty(nameof(Game.AwayTeam))]
        public virtual ICollection<Game> AwayGames { get; set; }
    }
}
