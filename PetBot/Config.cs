using Microsoft.Extensions.Configuration;

namespace PetBot
{
    /// <summary>
    /// Contains loaded user secrets
    /// </summary>
   public class Config
    {
      public static  string TelegramApiToken { get; private set; }
      public static string TelegramBotOwnerId { get; private set; }
      public static string GoogleSheetsDaoScriptId { get; private set; }


        static Config()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            TelegramApiToken = config["telegram_api_token"];
            TelegramBotOwnerId= config["admin_id"];
            GoogleSheetsDaoScriptId= config["google_script_id"];



        }
    }
}
