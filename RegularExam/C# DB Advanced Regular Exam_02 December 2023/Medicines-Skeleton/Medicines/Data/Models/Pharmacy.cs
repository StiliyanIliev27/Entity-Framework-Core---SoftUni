

using Medicines.Common;
using System.ComponentModel.DataAnnotations;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        public Pharmacy()
        {
            Medicines = new HashSet<Medicine>(); 
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PharmacyNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.PharmacyPhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public bool IsNonStop { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
