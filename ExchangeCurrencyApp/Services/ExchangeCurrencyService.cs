using ExchangeCurrencyApp.Configurations;
using ExchangeCurrencyApp.DTOs;
using System.Text.Json;

namespace ExchangeCurrencyApp.Services
{
    public class ExchangeCurrencyService : IExchangeCurrencyService
    {
        private readonly HttpClient httpClient;
        private readonly ExchangeRateApiConfig exchangeRateApiConfig;
        public ExchangeCurrencyService(IHttpClientFactory httpClientFactory, ExchangeRateApiConfig exchangeRateApiConfig)
        {
            httpClient = httpClientFactory.CreateClient(nameof(ExchangeCurrencyService));
            this.exchangeRateApiConfig = exchangeRateApiConfig;
        }

        public async Task<double> ConvertUsdToBrl(double usdValue)
        {
            using(var httpResponse = await httpClient.GetAsync($"/v6/{exchangeRateApiConfig.ApiKey}/latest/USD"))
            {
                if(httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var errorResult = await httpResponse.Content.ReadAsStringAsync();
                    throw new Exception(errorResult);
                }

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ExchangeRateResultDTO>(jsonString);
                var brlValueToOneUsd = result.Rates["BRL"];

                var resultInBrl = Math.Round(usdValue * brlValueToOneUsd, 2);

                return resultInBrl;
            }
        }
    }
}
