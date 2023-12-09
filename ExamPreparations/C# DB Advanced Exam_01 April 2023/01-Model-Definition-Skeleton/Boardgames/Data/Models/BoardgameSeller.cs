

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models
{
    public class BoardgameSeller
    {
        [Required]
        [ForeignKey(nameof(BoardgameId))]
        public int BoardgameId { get; set; }
        public virtual Boardgame Boardgame { get; set; }
        
        [Required]
        [ForeignKey(nameof(SellerId))]
        public int SellerId { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
