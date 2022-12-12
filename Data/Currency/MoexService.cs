using Data.Currency.InfoClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace Data.Currency
{
    public class MoexService:ICurrencyService
    {
        private readonly MoexCurrencyInfo _currencyInfo;
        public MoexService(MoexCurrencyInfo moexCurrency)
        {
            _currencyInfo = moexCurrency;

        }


        private string CreateUrl() => $"https://iss.moex.com/iss/engines/currency/markets/selt/boards/CETS/securities/{_currencyInfo.SelectorTextOrCode}.json";

        public double GetRate()
        {

            var json = Downloader.GetRequest(CreateUrl());
            if (!string.IsNullOrEmpty(json))
                try
                {
                    var obj = JObject.Parse(json);
                    var strRate = obj["marketdata"]?["data"]?[0]?[8]?.ToString();
                    double.TryParse(strRate, out double rate);

                    return _currencyInfo.ConvertToStandartView(rate);
                }
                catch (JsonReaderException ex)
                {
                    ILogger _logger = LogManager.GetCurrentClassLogger();
                    _logger.Error(ex, "can't parse json");
                }
            return 0;
        }

        public string ToBeautifulString ()
        {
            return GetRate().ToString("N2");
        }
        

    }
}
