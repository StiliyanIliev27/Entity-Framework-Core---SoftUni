using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            Bets = new List<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [MaxLength(Const.Constants.UsernameMaxLength)]
        public string? Username { get; set; }
        
        [MaxLength(Const.Constants.UsernameMaxLength)]
        public string? Name { get; set; }

        [Required]
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public decimal Balance { get; set; }
        public virtual ICollection<Bet> Bets { get; set;}
    }
}
