using System;

namespace Data.Currency.InfoClasses
{

    public class MirCurrencyInfo:CurrencyInfo
    {
        public MirCurrencyInfo(string value, Func<double, double> converter)
        {
            SelectorTextOrCode = value;
            ConvertToStandartView = converter;
        }
        public static MirCurrencyInfo Tenge => new MirCurrencyInfo("Казахстанский тенге", Inverse);
        public static MirCurrencyInfo BYN => new MirCurrencyInfo("Белорусский рубль", Streight);
        public static MirCurrencyInfo Dong => new MirCurrencyInfo("Вьетнамский донг", Inverse);
    }
}
