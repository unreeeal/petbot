using Data.Currency;

namespace PetBot.Currency
{
    public interface ICurrencyServiceResolver
    {
        /// <summary>
        /// Gets nedeed service or null
        /// </summary>
        /// <param name="text"></param>
        /// <returns>service or null</returns>
        ICurrencyService GetServiceByPartialName(string text);
        /// <summary>
        /// Gets supported services
        /// </summary>
        /// <returns>supported services</returns>
        string GetHelp();
    }
}
