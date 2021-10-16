using Domain;
using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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


       /* [Fact]
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
            string donationDate = "2021-10-13";

            // run request
            HttpResponseMessage responseWithDonationDate= client.GetAsync($"api/donations?date={donationDate}").Result;
            HttpResponseMessage responseWithProjectId = client.GetAsync($"api/donations?projectId={projectId}").Result;

            // process response
            var responseDataWithProjectId = responseWithProjectId.Content.ReadAsStringAsync().Result;
            var donationsWithProjectId = JsonConvert.DeserializeObject<List<Donation>>(responseDataWithProjectId);
            
            var responseDataWithDonationDate = responseWithProjectId.Content.ReadAsStringAsync().Result;
            var donationsWithDonationDate = JsonConvert.DeserializeObject<List<Donation>>(responseDataWithProjectId);

            // verify results
            Assert.Equal(HttpStatusCode.OK, responseWithProjectId.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseWithDonationDate.StatusCode);

            donationsWithProjectId.ForEach(x => Assert.Matches(projectId, x.ProjectId.ToString()));

            // TODO remove hardcoded time
            donationDate = "13/10/2021 12:31:39";
            donationsWithDonationDate.ForEach(x => Assert.Matches(donationDate, x.DonationDate.ToString()));
        }*/

        [Fact]
        public void FilterDonationsByProjectIdAndDonationDateFailure()
        {
            // setup
            string projectId = "Invalid ProjectId";
            string donationDate = "InvalidProjectId DonationDate";

            // run request
            HttpResponseMessage responseWithProjectId = client.GetAsync($"api/donations?projectId={projectId}").Result;
            HttpResponseMessage responseWithDonationDate = client.GetAsync($"api/donations?date={donationDate}").Result;

            // verify results
            Assert.Equal(HttpStatusCode.BadRequest, responseWithProjectId.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, responseWithDonationDate.StatusCode);
        }

        /* [Fact]
         public void GetDonationByDonationIdSuccess()
         {
             // setup
             string donationId = "89ae88a7-32be-49f2-b1c6-dea3c7097f32";
             string userId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";

             // run request
             HttpResponseMessage response = client.GetAsync($"api/donations/{donationId}?userId={userId}").Result;

             // process response
             var responseData = response.Content.ReadAsStringAsync().Result;
             var donation = JsonConvert.DeserializeObject<Donation>(responseData);

             // verify results
             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
             Assert.Matches(donationId, donation.DonationId.ToString());
             Assert.Matches(userId, donation.DonationDate.ToString());
         }*/

        [Fact]
        public void CreateDonationSuccess()
        {
            DonationDTO donationDTO = new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000);
            // setup
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
    }
}
