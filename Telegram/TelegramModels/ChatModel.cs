using Newtonsoft.Json;

namespace Telegram.TelegramModels
{
    public class Chat
    {//ff
        public string Id { get; set; }
        public string Type {get;set;}

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title{get;set;}
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string username {get;set;}
        [JsonProperty("First_Name",NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName{get;set;}
        [JsonProperty("Last_Name",NullValueHandling = NullValueHandling.Ignore)]
        public string LastName {get;set;}


    }
}
