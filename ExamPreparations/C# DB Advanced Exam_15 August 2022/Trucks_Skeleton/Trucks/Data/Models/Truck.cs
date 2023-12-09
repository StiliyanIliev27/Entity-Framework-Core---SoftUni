
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public Truck()
        {
             ClientsTrucks = new HashSet<ClientTruck>();   
        }
       
        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.RegistrationNumberLength)]
        public string? RegistrationNumber { get; set; }

        [MaxLength(ValidationConstants.VinNumberLength)]
        [Required]
        public string VinNumber { get; set; } = null!;

        [MaxLength(ValidationConstants.TankCapacityMaxLength)]
        [Required]
        public int TankCapacity { get; set; }

        [MaxLength(ValidationConstants.CargoCapacityMaxLength)]
        [Required]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(DespatcherId))]
        [Required]
        public int DespatcherId { get; set; }

        [Required]
        public virtual Despatcher Despatcher { get; set; } = null!;
        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
