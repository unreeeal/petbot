using System;

namespace Data.Currency.InfoClasses
{
    public class MoexCurrencyInfo : CurrencyInfo
    {
        private const string TENGE_RUB_CODE = "KZTRUB_TOM";
        private const string USD_RUB_CODE = "USD000000TOD";
        private const string EUR_RUB_CODE = "EUR_RUB__TOM";
        private const string BYN_RUB_CODE = "BYNRUB_TOM";
        

        private MoexCurrencyInfo(string value, Func<double, double> converter)
        {
            SelectorTextOrCode = value;
            ConvertToStandartView = converter;
        }

        public static MoexCurrencyInfo Tenge => new MoexCurrencyInfo(TENGE_RUB_CODE,(d)=> d==0 ? 0 : (100 / d));
        public static MoexCurrencyInfo USD => new MoexCurrencyInfo(USD_RUB_CODE, Streight);
        public static MoexCurrencyInfo EUR => new MoexCurrencyInfo(EUR_RUB_CODE, Streight);
        public static MoexCurrencyInfo BYN => new MoexCurrencyInfo(BYN_RUB_CODE, Streight);


    }
}
