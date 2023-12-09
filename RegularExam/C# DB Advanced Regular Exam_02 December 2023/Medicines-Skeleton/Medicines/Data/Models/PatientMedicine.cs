

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models
{
    public class PatientMedicine
    {
        [Required]
        [ForeignKey(nameof(PatientId))]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        [ForeignKey(nameof(MedicineId))]
        public int MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
