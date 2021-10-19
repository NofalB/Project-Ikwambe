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
        private string _token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImJiNzU5ZDFjLTFiM2YtNDlmMy1iNGYxLWY3OTE5MTAyYTZmZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RFIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJiYjc1OWQxYy0xYjNmLTQ5ZjMtYjRmMS1mNzkxOTEwMmE2ZmQiLCJuYmYiOjE2MzQ1NzAzNzUsImV4cCI6MTY4NjQxMDM3NSwiaWF0IjoxNjM0NTcwMzc1LCJpc3MiOiJEZWJ1Z0lzc3VlciIsImF1ZCI6IkRlYnVnQXVkaWVuY2UifQ.nvVcS52-ntRh1NwiBrMzLNYo6aLVhDSYPkf6fEBGOFA";

        public IntegrationTestDonation()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"https://stichting-ikwambe.azurewebsites.net/";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(hostname);

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", _token);
        }

        #region Sucesssful Tests
        [Fact]
        public void GetAllDonationsSuccess()
        {
            // setup
            string userId = "cafa35da-e093-4946-b50d-f4007d02843f";

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations?userId={userId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donations = JsonConvert.DeserializeObject<List<Donation>>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<List<Donation>>(donations);
        }

        [Fact]
        public void FilterDonationsByProjectIdAndDonationDateSuccess()
        {
            // setup
            string projectId = "787c7269-33cc-4198-a9d1-17bdade3a622";
            string donationDate = "2021-10-19";

            // run request
            HttpResponseMessage responseWithDonationDate = _httpClient.GetAsync($"api/donations?date={donationDate}").Result;
            HttpResponseMessage responseWithProjectId = _httpClient.GetAsync($"api/donations?projectId={projectId}").Result;

            // process response
            var responseDataByProjectId = responseWithProjectId.Content.ReadAsStringAsync().Result;
            var donationsByProjectId = JsonConvert.DeserializeObject<List<Donation>>(responseDataByProjectId);
            
            var responseDataByDate = responseWithDonationDate.Content.ReadAsStringAsync().Result;
            var donationsByDate = JsonConvert.DeserializeObject<List<Donation>>(responseDataByDate);

            // verify results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectId.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseWithDonationDate.StatusCode);

            donationsByProjectId.ForEach(x => Assert.Matches(projectId, x.ProjectId.ToString()));
            donationsByDate.ForEach(x => Assert.Matches(donationDate, x.DonationDate.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
        }

        [Fact]
        public void GetDonationByDonationIdSuccess()
        {
            // setup
            string donationId = "9404dc0b-1818-4b2e-b8d5-eb7782a01eea";
            string userId = "cafa35da-e093-4946-b50d-f4007d02843f";

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations/{donationId}?userId={userId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Matches(donationId, donation.DonationId.ToString());
            Assert.Matches(userId, donation.UserId.ToString());
        }

        [Fact]
        public void CreateDonationSuccess()
        {
            // setup
            DonationDTO donationDTO = new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000);
            HttpContent donationData = new StringContent(JsonConvert.SerializeObject(donationDTO), Encoding.UTF8, "application/json");

            // run request
            HttpResponseMessage response = _httpClient.PostAsync($"api/donations", donationData).Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<Donation>(donation);
        }

        #endregion


        #region Failed Tests
        [Fact]
        public void FilterDonationsByProjectIdAndDonationDateFailure()
        {
            // setup
            string projectId = "Invalid ProjectId";
            string donationDate = "InvalidProjectId DonationDate";

            // run request
            HttpResponseMessage responseByProjectId = _httpClient.GetAsync($"api/donations?projectId={projectId}").Result;
            HttpResponseMessage responseByDonationDate = _httpClient.GetAsync($"api/donations?date={donationDate}").Result;

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, responseByProjectId.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseByDonationDate.StatusCode);
        }

        [Fact]
        public void GetDonationByDonationIdFailure()
        {
            // setup
            string donationId = "Invalid donation ID";
            string userId = "Invalid user ID";

            // run request
            HttpResponseMessage response = _httpClient.GetAsync($"api/donations/{donationId}?userId={userId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Matches(donation.DonationId.ToString(), Guid.Empty.ToString());
        }

        [Fact]
        public void CreateDonationFailure()
        {
            // setup
            DonationDTO donationDTO = new DonationDTO(Guid.Empty, Guid.Empty, "", 0);
            HttpContent donationData = new StringContent(JsonConvert.SerializeObject(donationDTO), Encoding.UTF8, "application/json");
            
            // run request
            HttpResponseMessage response = _httpClient.PostAsync($"api/donations", donationData).Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(Guid.Empty.ToString(), donation.DonationId.ToString());
        }
        #endregion
    }
}
