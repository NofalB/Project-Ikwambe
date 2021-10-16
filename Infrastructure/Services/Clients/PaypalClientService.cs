using Domain;
using Infrastructure.Services.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Transactions
{
    public class PaypalClientService: IPaypalClientService
    {
        private readonly HttpClient _client;
        public PaypalClientService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new System.Uri("https://paypalmicroserviceikwambe.azurewebsites.net/api/");
        }

        public async Task<Transaction> CaptureTransaction(string orderId)
        {
            var TransactionData = await _client.GetAsync(_client.BaseAddress + "captureorder?orderId=" + orderId);
            var TransactionDataResponseObj = await TransactionData.Content.ReadAsStringAsync();
            var TransactionDataObj = JsonConvert.DeserializeObject<Transaction>(TransactionDataResponseObj);

            return TransactionDataObj;
        }

        public async Task<CheckoutUrl> GetCheckoutUrl(string currencyCode, string value)
        {
            var TransactionData = await _client.GetAsync(_client.BaseAddress+"createorder?currency=" + currencyCode + "&value=" + value);
            var TransactionDataResponseObj = await TransactionData.Content.ReadAsStringAsync();
            var TransactionDataObj = JsonConvert.DeserializeObject<CheckoutUrl>(TransactionDataResponseObj);

            return TransactionDataObj;
        }

        public async Task<Transaction> GetTransaction(string orderId)
        {
            var TransactionData = await _client.GetAsync(_client.BaseAddress + "getorder?orderId="+ orderId);
            var TransactionDataResponseObj = await TransactionData.Content.ReadAsStringAsync();
            var TransactionDataObj = JsonConvert.DeserializeObject<Transaction>(TransactionDataResponseObj);

            return TransactionDataObj;
        }
    }
}
