using Data.Currency.InfoClasses;
using System;
using Data.Currency;
using System.Collections.Generic;

namespace PetBot.Currency
{
    public class CurrencyServiceResolver : ICurrencyServiceResolver
    {

        private const string DEFAULT_ERROR_TEXT = "unknown service";
        private const string TRIM_ON_METHODS_NAME = "get_";
        private readonly string _typeName;
        public CurrencyServiceResolver(string typeName)
        {
            _typeName = typeName;
        }

        private List<string> GetSupportedCurrencies()
        {

            Type type;

            if (_typeName == nameof(MirService))
            {
                type = typeof(MirCurrencyInfo);
            }
            else if (_typeName == nameof(MoexService))
            {
                type = typeof(MoexCurrencyInfo);
            }
            else
                return null;


            var list = new List<string>();
            int trimOnStart = TRIM_ON_METHODS_NAME.Length;
            foreach (var line in type.GetMethods())
            {
                if (line.ReturnType == type)
                    list.Add(line.Name[trimOnStart..]);

            }
            return list;
        }

        public string GetHelp()
        {

            var supportedCurrencies = GetSupportedCurrencies();
            if (supportedCurrencies == null)
                return DEFAULT_ERROR_TEXT;
            else
                return "Supported currencies " + string.Join(", ", supportedCurrencies);
        }

        public ICurrencyService GetServiceByPartialName(string text)
        {

            text = text.ToLowerInvariant();
            if (_typeName == nameof(MirService))
            {
                return GetMirService(text);
            }
            if (_typeName == nameof(MoexService))
            {
                return GetMoexService(text);
            }
            return null;
        }

        private ICurrencyService GetMirService(string text)
        {

            MirCurrencyInfo currencyInfo;
            text = text.ToLowerInvariant();
            if (text.Contains("tenge"))
                currencyInfo = MirCurrencyInfo.Tenge;
            else if (text.Contains("byn"))
                currencyInfo = MirCurrencyInfo.BYN;
            else if (text.Contains("dong"))
                currencyInfo = MirCurrencyInfo.Dong;
            else
                return null;

            return new MirService(currencyInfo);

        }
        private ICurrencyService GetMoexService(string text)
        {
            MoexCurrencyInfo currencyInfo;


            if (text.Contains("usd"))
                currencyInfo = MoexCurrencyInfo.USD;
            else if (text.Contains("eur"))
                currencyInfo = MoexCurrencyInfo.EUR;
            else if (text.Contains("tenge"))
                currencyInfo = MoexCurrencyInfo.Tenge;
            else if (text.Contains("byn"))
                currencyInfo = MoexCurrencyInfo.BYN;
            else
                return null;

            return new MoexService(currencyInfo);
        }
    }
}
