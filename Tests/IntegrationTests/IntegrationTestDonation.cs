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
    public class IntegrationTestDonation
    {
        private HttpClient _httpClient { get; }

        #region Test IDs
        private string _userId = "36c98532-f489-4790-a02c-81e88a689950";
        private string _donationId = "267ddc35-1529-4052-9beb-e2cce641ce33";
        private string _projectId = "4ae756ac-b37f-4651-b718-9d6b916b7f1e";
        private string _donationDate = "2021-10-22";

        private string _falseUserId = "Invalid user ID";
        private string _falseProjectId = "Invalid ProjectId";
        private string _falseDonationId = "Invalid donation ID";
        private string _falseDonationDate = "InvalidProjectId DonationDate";
        #endregion

        private string _userToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IjM2Yzk4NTMyLWY0ODktNDc5MC1hMDJjLTgxZTg4YTY4OTk1MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InVzZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjM2Yzk4NTMyLWY0ODktNDc5MC1hMDJjLTgxZTg4YTY4OTk1MCIsIm5iZiI6MTYzNDg1MzA0MiwiZXhwIjoxNjg2NjkzMDQyLCJpYXQiOjE2MzQ4NTMwNDIsImlzcyI6IkRlYnVnSXNzdWVyIiwiYXVkIjoiRGVidWdBdWRpZW5jZSJ9.XetnRbFBJwhvOJQauam80MF1t8hqxhXurBT3s7G0zJA";
        private string _adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiIzNjJiZjIxMi1hNWJjLTQ5ZTQtOTRlYi01ZTVkM2ExZmJmODYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMzYyYmYyMTItYTViYy00OWU0LTk0ZWItNWU1ZDNhMWZiZjg2IiwibmJmIjoxNjM0ODUzMDEwLCJleHAiOjE2ODY2OTMwMTAsImlhdCI6MTYzNDg1MzAxMCwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.afTG3OzeVVOkRaMuNXXtQqpUu5OcoQQD3UmyrjFvPAk";

        public IntegrationTestDonation()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"http://localhost:{7071}";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _userToken);
        }

        #region Sucesssful Tests
        [Fact]
        public void FilterDonationsByProjectIdAndDonationDateSuccess()
        {
            // setup
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            // run request
            HttpResponseMessage responseWithDonationDate = _httpClient.GetAsync($"api/donations?donationDate={_donationDate}").Result;
            HttpResponseMessage responseWithProjectId = _httpClient.GetAsync($"api/donations?projectId={_projectId}").Result;
            
            // process response
            var responseDataByProjectId = responseWithProjectId.Content.ReadAsStringAsync().Result;
            var donationsByProjectId = JsonConvert.DeserializeObject<List<Donation>>(responseDataByProjectId);
            
            var responseDataByDate = responseWithDonationDate.Content.ReadAsStringAsync().Result;
            var donationsByDate = JsonConvert.DeserializeObject<List<Donation>>(responseDataByDate);

            // verify results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectId.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseWithDonationDate.StatusCode);

            donationsByProjectId.ForEach(x => Assert.Matches(_projectId, x.ProjectId.ToString()));
            donationsByDate.ForEach(x => Assert.Matches(_donationDate, x.DonationDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
        }

        [Fact]
        public void GetDonationByDonationIdSuccess()
        {
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations/{_donationId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Matches(_donationId, donation.DonationId.ToString());
        }

        [Fact]
        public void GetAllDonationsByUserIdSuccess()
        {
            // setup
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
            
            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations/user/{_userId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donations = JsonConvert.DeserializeObject<List<Donation>>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<List<Donation>>(donations);
            donations.ForEach(x => Assert.Matches(_userId, x.UserId.ToString()));
        }

        #endregion


        #region Failed Tests
        [Fact]
        public void FilterDonationsByProjectIdAndDonationDateFailure()
        {
            // setup
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            // run request
            HttpResponseMessage responseByProjectId = _httpClient.GetAsync($"api/donations?projectId={_falseProjectId}").Result;
            HttpResponseMessage responseByDonationDate = _httpClient.GetAsync($"api/donations?donationDate={_falseDonationDate}").Result;

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, responseByProjectId.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseByDonationDate.StatusCode);
        }

        [Fact]
        public void GetDonationByDonationIdFailure()
        {
            // setup
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations/{_falseDonationId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //Assert.Matches(donation.DonationId.ToString(), Guid.Empty.ToString());
        }

        [Fact]
        public void GetAllDonationsByUserIdFailure()
        {
            // setup
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations/user/{_falseUserId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donations = JsonConvert.DeserializeObject(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            //Assert.NotNull(donations);
        }
        #endregion
    }
}
