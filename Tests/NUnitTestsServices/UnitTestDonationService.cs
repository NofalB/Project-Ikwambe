using AutoMapper;
using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MockQueryable.Moq;
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
        private string _testUserId;

        private DonationService _donationService;
        private List<Donation> _mockListDonations;
        private Mock<IUserService> _userServiceMock;
        private Mock<IWaterpumpProjectService> _waterpumpProjectServiceMock;
        private Mock<ICosmosReadRepository<Donation>> _donationReadRepositoryMock;
        private Mock<ICosmosWriteRepository<Donation>> _donationWriteRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _testUserId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";

            _userServiceMock = new Mock<IUserService>();
            _waterpumpProjectServiceMock = new Mock<IWaterpumpProjectService>();

            _donationReadRepositoryMock = new Mock<ICosmosReadRepository<Donation>>();
            _donationWriteRepositoryMock = new Mock<ICosmosWriteRepository<Donation>>();

            _donationService = new DonationService(_donationReadRepositoryMock.Object, _donationWriteRepositoryMock.Object,
                _userServiceMock.Object, _waterpumpProjectServiceMock.Object);

            // set up mock donation data
            _mockListDonations = new List<Donation>()
            {
                new Donation(Guid.NewGuid(), Guid.Parse(_testUserId), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
            };
        }

        [Test]
        public void GetAllDonations_Should_Return_All_MockDonationsAsync()
        {
            //Arrange
            var mockDonations = _mockListDonations.AsQueryable().BuildMockDbSet();
            _donationReadRepositoryMock.Setup(d => d.GetAll()).Returns(mockDonations.Object);

            //Act
            var donations = _donationService.GetDonationByUserIdAsync(_testUserId).Result.ToList();

            //Assert
            Assert.IsNotNull(donations);
            Assert.That(donations, Is.InstanceOf(typeof(IEnumerable<Donation>)));
            Assert.AreEqual(1, donations.Count);

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