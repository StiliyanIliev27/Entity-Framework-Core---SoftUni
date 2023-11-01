using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            Players = new List<Player>();
        }

        [Key]
        public int PositionId { get; set; }

        [MaxLength(Const.Constants.PositionNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;
        public virtual ICollection<Player> Players { get; set;}
    }
}
