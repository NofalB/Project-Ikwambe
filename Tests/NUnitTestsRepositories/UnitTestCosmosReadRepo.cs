using Domain;
using Infrastructure.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTestsRepositories
{
    public class UnitTestCosmosReadRepo
    {
        private IQueryable<Donation> _mockListDonations;
        private Mock<ICosmosReadRepository<Donation>> _donationReadMock;
        private ICosmosReadRepository<Donation> _donationReadRepo;

        [SetUp]
        public void Setup()
        {
            _donationReadMock = new Mock<ICosmosReadRepository<Donation>>();

            _donationReadRepo = _donationReadMock.Object;

            // set up mock donation data
            _mockListDonations =
                (new List<Donation> {
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
                }).AsQueryable();

        }

        [Test]
        public void GetAll_Should_Return_All_MockListDonations()
        {

            //All Donations
            _donationReadMock.Setup(m => m.GetAll()).Returns(_mockListDonations);

            //Act
            var testDives = (IList<Donation>)_donationReadRepo.GetAll().ToList();

            //Assert
            Assert.AreEqual(2, testDives.Count);
        }
    }
}