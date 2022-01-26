using Domain;
using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class IntegrationTestTransaction
    {
        private HttpClient _httpClient { get; }

        #region Test IDs
        private string _testTransactionId = "4KN4384602045020Y";
        private string _testProjectId = "4ae756ac-b37f-4651-b718-9d6b916b7f1e";
        private string _testUserId = "36c98532-f489-4790-a02c-81e88a689950";

        private string _falseTransactionId = "Invalid Transaction ID";
        private string _falseProjectId = "Invalid Project ID";
        private string _falseUserId = "Invalid User ID";
        #endregion

        public IntegrationTestTransaction()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"http://localhost:{7071}";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);
        }

        #region Sucesssful Tests
        [Fact]
        public void GetAllTransactionsSuccess()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/db").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<List<Transaction>>(transactions);
        }

        [Fact]
        public void GetTransactionByIdSuccess()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/db/{_testTransactionId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var transaction = JsonConvert.DeserializeObject<Transaction>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<Transaction>(transaction);
            Assert.Matches("4KN4384602045020Y", transaction.TransactionId);
        }
        
        [Fact]
        public void GetTransactionsPayPalSuccess()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/paypal/{_testTransactionId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var transaction = JsonConvert.DeserializeObject<Transaction>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<Transaction>(transaction);
            Assert.Matches("4KN4384602045020Y", transaction.TransactionId);
        }

        [Fact]
        public void CreateCheckoutUrlSuccess()
        {
            // setup
            string currency = "EUR";
            string val = "50";

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/paypal/checkout?currency={currency}&value={val}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var checkoutUrl = JsonConvert.DeserializeObject<CheckoutUrl>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(checkoutUrl);
            Assert.Matches("CREATED", checkoutUrl.Status);
            Assert.Matches("Here is the link to make the payment:", checkoutUrl.Message);
        }

        [Fact]
        public void CompleteTransactionSuccess()
        {
            // setup
            DonationDTO TestDonation = new DonationDTO()
            {
                ProjectId = Guid.Parse("4ae756ac-b37f-4651-b718-9d6b916b7f1e"),
                TransactionId = "7H726527DP790725L",
                Comment = "Test donation",
                Name = "someone"
            };

            HttpContent donationData = new StringContent(JsonConvert.SerializeObject(TestDonation), Encoding.UTF8, "application/json");
            
            // run request
            HttpResponseMessage response = _httpClient.PostAsync($"api/transactions/paypal/complete1", donationData).Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;

            // verify results
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Matches("Transaction done successfully!", responseData);
        }
        #endregion

        #region Failed Tests
        [Fact]
        public void GetTransactionByIdFailure()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/db/{_falseTransactionId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void GetTransactionsPayPalFailure()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/paypal/{_falseTransactionId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;

            // verify results
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        }

        [Fact]
        public void CreateCheckoutUrlFailure()
        {
            // setup
            string currency = "False Currency";
            string val = "False Value";

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/paypal/checkout?currency={currency}&value={val}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;

            // verify results
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void CompleteTransactionFailure()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/transactions/paypal/complete?" +
                $"transactionId={_falseTransactionId}&projectId={_falseProjectId}&userId={_falseUserId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;

            // verify results
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        #endregion
    }
}
