using Data.Currency.InfoClasses;
using HtmlAgilityPack;

namespace Data.Currency
{

    /// <summary>
    /// Service to get Mir payment rates
    /// </summary>
    public class MirService:ICurrencyService
    {
        private const string MAIN_URL = "https://mironline.ru/support/list/kursy_mir/";

        private readonly MirCurrencyInfo _currencyInfo;

        /// <summary>
        /// Create an instance of Mir service
        /// </summary>
        /// <param name="currency">MircurrencyInfo </param>
        public MirService(MirCurrencyInfo currency)
        {
            _currencyInfo = currency;
        }
        /// <summary>
        /// Current rate in rubles, updates 9am every day.
        /// </summary>
        /// <returns>current rate in rubles </returns>
        public  double GetRate()
        {
            var ps = Downloader.GetRequest(MAIN_URL);
            if (string.IsNullOrEmpty(ps))
                return 0;
            var html = new HtmlDocument();
            html.LoadHtml(ps);
            var doc = html.DocumentNode;
            var strRate = doc.SelectSingleNode($"//p[contains(text(),'{_currencyInfo.SelectorTextOrCode}')]/../../td[2]")?.InnerText.Trim();
            double.TryParse(strRate, out double rate);
            return  _currencyInfo.ConvertToStandartView(rate);
            

        }
        /// <summary>
        /// Converts to string with two decimal places
        /// </summary>
        /// <returns>current rate with two decimal places</returns>
        public string ToBeautifulString()
        {
            return GetRate().ToString("N2");
        }

    }
}
