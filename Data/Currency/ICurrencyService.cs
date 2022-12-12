namespace Data.Currency
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Returns current rate in rubles 
        /// </summary>
        /// <returns>Current rate in rubles</returns>
        double GetRate();
        /// <summary>
        /// Create a Beautiful string  with current rate
        /// </summary>
        /// <returns>Beatiful rate in Rubles</returns>
        string ToBeautifulString();

    }
}
