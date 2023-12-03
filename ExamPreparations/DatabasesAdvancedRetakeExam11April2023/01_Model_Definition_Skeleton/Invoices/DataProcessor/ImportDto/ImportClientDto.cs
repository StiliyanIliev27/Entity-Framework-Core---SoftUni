

using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientDto
    {
        [Required]
        [MaxLength(25)]
        [MinLength(10)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        [MinLength(10)]
        public string NumberVat { get; set; }

        [XmlArray("Addresses")]
        public ImportAddressDto[] Addresses { get; set; }
    }
}
