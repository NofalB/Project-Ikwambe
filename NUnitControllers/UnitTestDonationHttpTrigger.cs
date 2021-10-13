using Domain;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
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
       // private HttpClient _httpClient;

        private List<Donation> _mockLstDonations;
        private DonationHttpTrigger _donationHttpTrigger;
        private Mock<IDonationService> _mockDonationService;
        private Mock<ILogger<DonationHttpTrigger>> _mockLogger;

        private Mock<HttpRequestData> _mockReq;
        private Mock<FunctionContext> _mockExecutionContext;

        [SetUp]
        public void Setup()
        {
            /*_httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:7071/");*/

            _mockReq = new Mock<HttpRequestData>();
            _mockExecutionContext = new Mock<FunctionContext>();

            // set up mock donation trigger
            _mockLogger = new Mock<ILogger<DonationHttpTrigger>>();
            _mockDonationService = new Mock<IDonationService>();
            _donationHttpTrigger = new DonationHttpTrigger(_mockLogger.Object, _mockDonationService.Object);

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
            var donations = _donationHttpTrigger.GetDonations(_mockReq.Object, _mockExecutionContext.Object).Result;

            // HttpResponseMessage response = _httpClient.GetAsync("api/donations").Result;
            // var donations = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.IsInstanceOf<IEnumerable<Donation>>(donations);
            Assert.IsNotNull(donations);
            Assert.AreEqual(HttpStatusCode.OK, donations.StatusCode);
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //CollectionAssert.AreEqual(donations, _mockLstDonations);
        }
    }
}