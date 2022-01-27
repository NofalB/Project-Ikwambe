using Domain;
using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class IntegrationTestTransaction
    {
        private HttpClient _httpClient { get; }
        private string _adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiIzNjJiZjIxMi1hNWJjLTQ5ZTQtOTRlYi01ZTVkM2ExZmJmODYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMzYyYmYyMTItYTViYy00OWU0LTk0ZWItNWU1ZDNhMWZiZjg2IiwibmJmIjoxNjM0ODUzMDEwLCJleHAiOjE2ODY2OTMwMTAsImlhdCI6MTYzNDg1MzAxMCwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.afTG3OzeVVOkRaMuNXXtQqpUu5OcoQQD3UmyrjFvPAk";

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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

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
