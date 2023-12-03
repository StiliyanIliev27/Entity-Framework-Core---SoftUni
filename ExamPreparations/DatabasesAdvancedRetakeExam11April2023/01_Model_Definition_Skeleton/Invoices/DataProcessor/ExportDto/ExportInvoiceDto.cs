
using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Invoice")]
    public class ExportInvoiceDto
    {
        [XmlElement("InvoiceNumber")]
        public int Number { get; set; }

        [XmlElement("InvoiceAmount")]
        public double Amount { get; set; }
        
        public string DueDate { get; set; }

        [XmlElement("Currency")]
        public CurrencyType CurrencyType { get; set; }
    }
}
