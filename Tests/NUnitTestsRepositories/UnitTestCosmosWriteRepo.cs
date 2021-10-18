using Domain;
using Infrastructure.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTestsRepositories
{
    public class UnitTestCosmosWriteRepocs
    {
        private IQueryable<Donation> _mockListDonations;
        private Mock<ICosmosWriteRepository<Donation>> _donationWriteMock;
        private ICosmosWriteRepository<Donation> _donationWriteRepo;


        private Mock<ICosmosReadRepository<Donation>> _donationReadMock;
        private ICosmosReadRepository<Donation> _donationReadRepo;

        [SetUp]
        public void Setup()
        {
            _donationReadMock = new Mock<ICosmosReadRepository<Donation>>();
            _donationReadRepo = _donationReadMock.Object;

            _donationWriteMock = new Mock<ICosmosWriteRepository<Donation>>();
            _donationWriteRepo = _donationWriteMock.Object;

            //Add Dive
            _donationWriteMock.Setup(m => m.AddAsync(It.IsAny<Donation>())).Callback(new Action<Donation>(
                        x =>
                        {
                            _mockListDonations.ToList().Add(x);
                        }
                    ));

            // set up mock donation data
            _mockListDonations =
                (new List<Donation> {
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
                }).AsQueryable();

            _donationReadMock.Setup(m => m.GetAll()).Returns(_mockListDonations);
        }

        [Test]
        public void Insert_Donation_Should_Return_Increased_MockListDonations()
        {
            // Arrange
            Donation testDonation = new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 200);

            //Act
            _donationWriteRepo.AddAsync(testDonation);
            var after = (IList<Donation>)_donationReadRepo.GetAll().ToList();

            //Assert
            Assert.AreEqual(3, after.Count);
        }
    }
}