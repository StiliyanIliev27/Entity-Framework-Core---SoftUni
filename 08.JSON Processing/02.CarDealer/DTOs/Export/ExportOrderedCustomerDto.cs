using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Export
{
    public class ExportOrderedCustomerDto
    {
        [JsonProperty("Name")]
        public string CustomerName { get; set; } = null!;

        [JsonProperty("BirthDate")]
        public DateTime CustomerBirthDate { get; set; }

        [JsonProperty("IsYoungDriver")]
        public bool CustomerIsYoungDriver { get; set; }
    }
}
