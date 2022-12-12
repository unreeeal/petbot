using Newtonsoft.Json;

namespace Telegram.TelegramModels
{
    public class UpdateModel
    {
        [JsonProperty("Update_id")]
        public int UpdateId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MessageModel Message { get; set; }
       
        //public CallbackQuery Callback_Query {get;set;}
    }
}
