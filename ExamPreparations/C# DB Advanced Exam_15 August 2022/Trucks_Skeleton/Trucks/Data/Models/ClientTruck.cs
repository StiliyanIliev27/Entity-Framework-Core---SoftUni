

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        [Required]
        [ForeignKey(nameof(ClientId))]
        public int ClientId { get; set; }

        [Required]
        public virtual Client Client { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(TruckId))]
        public int TruckId { get; set; }

        [Required]
        public virtual Truck Truck { get; set; } = null!;
    }
}
