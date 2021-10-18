using AutoMapper;
using Domain;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestsServices
{
    public class UnitTestDonationService
    {
        private DonationService _donationService;
        private IQueryable<Donation> _mockListDonations;
        private Mock<ICosmosReadRepository<Donation>> _donationReadRepositoryMock;
        private Mock<ICosmosWriteRepository<Donation>> _donationWriteRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _donationReadRepositoryMock = new Mock<ICosmosReadRepository<Donation>>();
            _donationWriteRepositoryMock = new Mock<ICosmosWriteRepository<Donation>>();

            _donationService = new DonationService(_donationReadRepositoryMock.Object, _donationWriteRepositoryMock.Object);

            // set up mock donation data
            /*_mockListDonations = new List<Donation>()
            {
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
            };*/

            _mockListDonations =
                (new List<Donation> {
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
                }).AsQueryable();
        }

        [Test]
        public void GetAllDonations_Should_Return_All_MockDonationsAsync()
        {
            //Arrange
            _donationReadRepositoryMock.Setup(d => d.GetAll()).Returns(_mockListDonations);

            //Act
            IEnumerable<Donation> donations = _donationService.GetAllDonationsAsync().Result;

            //Assert
            Assert.IsNotNull(donations);
            Assert.That(donations, Is.InstanceOf(typeof(IEnumerable<Donation>)));

            //Check that the GetAll method was called once
            _donationReadRepositoryMock.Verify(d => d.GetAll(), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _donationReadRepositoryMock = null;
            _donationWriteRepositoryMock = null;
            _mockListDonations = null;
        }
    }
}