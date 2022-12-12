using Data.Currency;
using PetBot.Currency;

namespace PetBot.CommandHandlers
{
    public class CurrencyHandler
    {
        private const string DEFAULT_ERROR_TEXT= "Can't find your currency";
        /// <summary>
        /// Parses command to get rate of needed currency, support Moex and Mir Payment
        /// </summary>
        /// <param name="text">command should be like /mir tenge .... /moex usd</param>
        /// <returns>rates or help in case of fail</returns>
        public static string ParseCommand(string text)
        {
           
            string typeName;
            if (text.Contains("moex"))
                typeName = nameof(MoexService);
            else if (text.Contains("mir"))
                typeName = nameof(MirService);

            else
                return DEFAULT_ERROR_TEXT;
                        ICurrencyServiceResolver serviceResolver = new CurrencyServiceResolver(typeName);
            ICurrencyService service = serviceResolver.GetServiceByPartialName(text);
            if (service != null)
                return service.ToBeautifulString();


            return serviceResolver.GetHelp();




        }
    }
}
