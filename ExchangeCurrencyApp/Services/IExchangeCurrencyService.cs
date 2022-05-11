namespace ExchangeCurrencyApp.Services
{
    public interface IExchangeCurrencyService
    {
        Task<double> ConvertUsdToBrl(double usdValue);
    }
}
