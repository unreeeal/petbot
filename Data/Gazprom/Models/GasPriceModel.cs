using Newtonsoft.Json;

namespace Data.Models
{
    public class GasPriceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Price { get; set; }
    }
}
