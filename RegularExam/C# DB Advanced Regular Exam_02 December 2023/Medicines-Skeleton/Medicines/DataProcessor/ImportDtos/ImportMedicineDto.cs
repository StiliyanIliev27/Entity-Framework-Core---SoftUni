

using Medicines.Common;
using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDto
    {
        [Required]
        [Range(0, 4)]
        [XmlAttribute("category")]
        public int Category { get; set; }

        [Required]
        [MinLength(ValidationConstants.MedicineNameMinLength)]
        [MaxLength(ValidationConstants.MedicineNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.MedicinePriceMinValue,
            ValidationConstants.MedicinePriceMaxValue)]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProductionDate")]
        public string ProductionDate { get; set; } = null!;

        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MedicineProducerMinValue)]
        [MaxLength(ValidationConstants.MedicineProducerMaxValue)]
        [XmlElement("Producer")]
        public string Producer { get; set; } = null!;
    }
}
