

using Medicines.Data.Models;
using Newtonsoft.Json;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportMedicineDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Pharmacy")]
        public ExportPharmacyDto Pharmacy { get; set; }
    }
}
