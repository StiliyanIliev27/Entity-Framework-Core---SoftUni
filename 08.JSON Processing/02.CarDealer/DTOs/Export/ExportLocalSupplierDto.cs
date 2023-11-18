using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Export
{
    public class ExportLocalSupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int PartsCount { get; set; }
    }
}
