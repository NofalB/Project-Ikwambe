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
            value = !string.IsNullOrEmpty(value) ? value : throw new ArgumentNullException("No value was provided.");
            currencyCode = !string.IsNullOrEmpty(currencyCode) ? currencyCode : throw new ArgumentNullException("No currencyCode was provided.");

            var doubleValue = double.Parse(value);
            if (doubleValue <= 0)
            {
                throw new InvalidOperationException($"The provided donation value can not be less than zero");
            }
            var TransactionData = await _client.GetAsync(_client.BaseAddress+"createorder?currency=" + currencyCode + "&value=" + value);
            var TransactionDataResponseObj = await TransactionData.Content.ReadAsStringAsync();
            var TransactionDataObj = JsonConvert.DeserializeObject<CheckoutUrl>(TransactionDataResponseObj);

            return TransactionDataObj;
        }

        public async Task<Transaction> GetTransaction(string transactionId)
        {
            transactionId = !string.IsNullOrEmpty(transactionId) ? transactionId : throw new ArgumentNullException("No value was provided.");

            var TransactionData = await _client.GetAsync(_client.BaseAddress + "getorder?orderId="+ transactionId);
            var TransactionDataResponseObj = await TransactionData.Content.ReadAsStringAsync();
            var TransactionDataObj = JsonConvert.DeserializeObject<Transaction>(TransactionDataResponseObj) ?? throw new ArgumentNullException($"The transaction with the specified id {transactionId} does not exist"); ;

            return TransactionDataObj;
        }

        public static string GetEnvironmentVariable(string name)
        {
            return name + ": " +
                System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
