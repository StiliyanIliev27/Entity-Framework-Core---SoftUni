

using Medicines.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.PharmacyNameMinLength)]
        [MaxLength(ValidationConstants.PharmacyNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]        
        [RegularExpression(ValidationConstants.PharmacyPhoneNumberRegEx)]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray("Medicines")]
        public ImportMedicineDto[] Medicines { get; set; }
    }
}
