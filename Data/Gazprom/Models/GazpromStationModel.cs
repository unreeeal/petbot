using Newtonsoft.Json;

namespace Data.Models
{
    public class GazpromStationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Identifier { get; set; }
        public int Region { get; set; }
        public string WayId { get; set; }
        public string Brand { get; set; }
        public string Place { get; set; }
        public double AAZS { get; set; }

        [JsonProperty("FUELS_DATA")]
        public GasPriceModel[] GasPriceArray { get; set; }
        public double Pricetime { get; set; }

    }
}
