
using Medicines.Common;
using Medicines.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MinLength(ValidationConstants.PatientFullNameMinLength)]
        [MaxLength(ValidationConstants.PatientFullNameMaxLength)]
        [JsonProperty("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(0, 2)]
        [JsonProperty("AgeGroup")]
        public int AgeGroup { get; set; }

        [Required]
        [Range(0, 1)]
        [JsonProperty("Gender")]
        public int Gender { get; set; }

        [JsonProperty("Medicines")]
        public int[] Medicines { get; set; }
    }
}
