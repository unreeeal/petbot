using Newtonsoft.Json;
namespace Telegram.TelegramModels
{
    public class MessageModel
    {
        [JsonProperty("Message_id")]
        public int MessageId { get; set; }
        public UserModel User { get; set; }
        public Chat Chat { get; set; }

        public int Date { get; set; }
        public string Text { get; set; }
    }
}
