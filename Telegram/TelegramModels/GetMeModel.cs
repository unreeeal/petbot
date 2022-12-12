using Newtonsoft.Json;

namespace Telegram.TelegramModels
{
  public  class GetMeModel
    {
        
        //[JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        public string UserName { get; set; }

    }
}
