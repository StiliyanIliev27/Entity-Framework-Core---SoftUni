

using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientDto
    {
        [XmlAttribute("Gender")]
        public string Gender { get; set; } = null!;

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; } = null!;

        [XmlArray("Medicines")]
        public ExportPatientMedicineDto[] Medicines { get; set; }
    }
}
