using System;

namespace Data.Currency.InfoClasses
{
    public abstract class CurrencyInfo
    {
     /// <summary>
     /// Holds selector text or identifier code
     /// </summary>
        public string SelectorTextOrCode { get; protected set; }
        /// <summary>
        /// Function to convert in standart view
        /// </summary>
        public Func<double, double> ConvertToStandartView { get; protected set; }
        protected readonly static Func<double, double> Inverse = (d) => d == 0 ? 0 : 1 / d;
        protected readonly static Func<double, double> Streight = (d) => d;
    }
}
