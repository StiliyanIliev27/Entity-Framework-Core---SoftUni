

using System.ComponentModel.DataAnnotations;
using Trucks.Common;

namespace Trucks.Data.Models
{
    public class Client
    {
        public Client()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.ClientNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
        [Required]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;
        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
