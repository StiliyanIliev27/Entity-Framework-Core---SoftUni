

using Newtonsoft.Json;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Trucks")]
        public ExportTruckDto[] Trucks { get; set; }
    }
}
