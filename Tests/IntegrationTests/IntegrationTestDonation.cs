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
    public class IntegrationTestDonation
    {
        private HttpClient client { get; }


        public IntegrationTestDonation()
        {
            string hostname = Environment.GetEnvironmentVariable("functionHostName");

            if (hostname == null)
                hostname = $"http://localhost:{7071}";
            client = new HttpClient();
            client.BaseAddress = new Uri(hostname);
        }

        #region Sucesssful Tests
        [Fact]
        public void GetAllDonationsSuccess()
        {
            // run request
            HttpResponseMessage response = client.GetAsync("api/donations").Result;

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
            string projectId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            string donationDate = "2021-10-16";

            // run request
            HttpResponseMessage responseWithDonationDate = client.GetAsync($"api/donations?date={donationDate}").Result;
            HttpResponseMessage responseWithProjectId = client.GetAsync($"api/donations?projectId={projectId}").Result;

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
            string donationId = "89ae88a7-32be-49f2-b1c6-dea3c7097f32";
            string userId = "bb759d1c-1b3f-49f3-b4f1-f7919102a6fd";

            // run request
            HttpResponseMessage response = client.GetAsync($"api/donations/{donationId}?userId={userId}").Result;

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
            HttpResponseMessage response = client.PostAsync($"api/donations", donationData).Result;

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
            HttpResponseMessage responseByProjectId = client.GetAsync($"api/donations?projectId={projectId}").Result;
            HttpResponseMessage responseByDonationDate = client.GetAsync($"api/donations?date={donationDate}").Result;

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
            HttpResponseMessage response = client.GetAsync($"api/donations/{donationId}?userId={userId}").Result;

            // process response
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donation = JsonConvert.DeserializeObject<Donation>(responseData);

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.DoesNotMatch(donationId, donation.DonationId.ToString());
            Assert.DoesNotMatch(userId, donation.UserId.ToString());
        }

        [Fact]
        public void CreateDonationFailure()
        {
            // setup
            DonationDTO donationDTO = new DonationDTO(Guid.Empty, Guid.Empty, "", 0);
            HttpContent donationData = new StringContent(JsonConvert.SerializeObject(donationDTO), Encoding.UTF8, "application/json");
            
            // run request
            HttpResponseMessage response = client.PostAsync($"api/donations", donationData).Result;

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
