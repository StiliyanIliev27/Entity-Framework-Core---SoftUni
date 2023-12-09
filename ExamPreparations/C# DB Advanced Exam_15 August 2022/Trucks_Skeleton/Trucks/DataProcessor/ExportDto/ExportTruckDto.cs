

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportTruckDto
    {
        [JsonProperty("TruckRegistrationNumber")]
        public string? TruckRegistrationNumber { get; set; }

        [JsonProperty("VinNumber")]
        public string VinNumber { get; set; } = null!;

        [JsonProperty("TankCapacity")]
        public int TankCapacity { get; set; }

        [JsonProperty("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [JsonProperty("CategoryType")]
        public string CategoryType { get; set; }

        [JsonProperty("MakeType")]
        public string MakeType { get; set; }
    }
}
