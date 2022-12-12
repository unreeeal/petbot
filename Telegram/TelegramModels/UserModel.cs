using Newtonsoft.Json;

namespace Telegram.TelegramModels
{
    public class UserModel
    {
        public int Id { get; set; }
        [JsonProperty("First_Name")]
        public string FirstName { get; set; }

        [JsonProperty("Last_Name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }
    }
}
