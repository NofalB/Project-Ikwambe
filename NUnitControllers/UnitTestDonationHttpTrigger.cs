using Domain;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectIkwambe.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace NUnitControllers
{
    public class Tests
    {
        private HttpClient _httpClient;
        private List<Donation> _mockLstDonations;
        private Mock<IDonationService> _mockDonationService;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:7071/");

            // set up mock donation trigger
            _mockDonationService = new Mock<IDonationService>();

            // set up mock donation data
            _mockLstDonations = new List<Donation>()
            {
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 4000),
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 599)
            };
        }

        [Test]
        public void GetAllDonations_Should_Return_All_MockDonations()
        {
            //Arrange 
            _mockDonationService.Setup(d => d.GetAllDonationsAsync().Result).Returns(_mockLstDonations);

            // Act
            HttpResponseMessage response = _httpClient.GetAsync("api/donations").Result;
            var responseData = response.Content.ReadAsStringAsync().Result;
            var donations = JsonConvert.DeserializeObject<List<Donation>>(responseData);

            // Assert
            Assert.IsNotNull(donations);
            Assert.IsInstanceOf<IEnumerable<Donation>>(donations);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}