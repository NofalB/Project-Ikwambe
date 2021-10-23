using AutoMapper;
using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Clients;
using Infrastructure.Services.Transactions;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestsServices
{
    public class UnitTestTransactionService
    {
        private string _testTransactionId;
        private List<Transaction> _mockListTransactions;

        private TransactionService _transactionService;
        private Mock<IUserService> _userServiceMock;
        private Mock<IDonationService> _donationServiceMock;
        private Mock<IPaypalClientService> _paypalClientServiceMock;
        private Mock<IWaterpumpProjectService> _waterpumpProjectServiceMock;
        private Mock<ICosmosReadRepository<Transaction>> _transactionReadRepositoryMock;
        private Mock<ICosmosWriteRepository<Transaction>> _transactionWriteRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _testTransactionId = "4KN4384602045020Y";

            _userServiceMock = new Mock<IUserService>();
            _donationServiceMock = new Mock<IDonationService>();
            _paypalClientServiceMock = new Mock<IPaypalClientService>();
            _waterpumpProjectServiceMock = new Mock<IWaterpumpProjectService>();

            _transactionReadRepositoryMock = new Mock<ICosmosReadRepository<Transaction>>();
            _transactionWriteRepositoryMock = new Mock<ICosmosWriteRepository<Transaction>>();

            _transactionService = new TransactionService(_transactionReadRepositoryMock.Object, 
                _transactionWriteRepositoryMock.Object,
                _paypalClientServiceMock.Object,
                _donationServiceMock.Object,
                _waterpumpProjectServiceMock.Object,
                _userServiceMock.Object
            );

            // setup transactions mock data
            SetupTransactionMockData();
        }

        private void SetupTransactionMockData()
        {
            Transaction testTransaction1 = new Transaction()
            {
                CreateTime = DateTime.Now,
                TransactionId = _testTransactionId,
                Intent = "CAPTURE",

                Payer = new Payer()
                {
                    Address = new Address()
                    {
                        CountryCode = "NL",
                        AddressId = "98fcaa5e-3b43-4e7a-a92c-7819a07ae7a1"
                    },
                    EmailAddress = "testTransaction1@example.com",
                    Name = new Name()
                    {
                        GivenName = "John",
                        Surname = "Doe",
                        FullName = "John Doe"
                    }
                }
            };

            Transaction testTransaction2 = new Transaction()
            {
                CreateTime = DateTime.Now,
                TransactionId = "1ASK3846020450AY31",
                Intent = "CAPTURE",

                Payer = new Payer()
                {
                    Address = new Address()
                    {
                        CountryCode = "NL",
                        AddressId = "98fcaa5e-3b43-4e7a-a92c-7819a07ae7a1"
                    },
                    EmailAddress = "testTransaction2@example.com",
                    Name = new Name()
                    {
                        GivenName = "Jane",
                        Surname = "Doe",
                        FullName = "Jane Doe"
                    }
                }
            };

            _mockListTransactions = new List<Transaction>();
            _mockListTransactions.Add(testTransaction1);
            _mockListTransactions.Add(testTransaction2);
        }

        [Test]
        public void GetTransactionsById_Should_Return_Specific_MockTransactionAsync()
        {
            //Arrange
            var mockTransaction = _mockListTransactions.AsQueryable().BuildMockDbSet();
            _transactionReadRepositoryMock.Setup(t => t.GetAll()).Returns(mockTransaction.Object);

            //Act
            var transaction = _transactionService.GetTransactionById(_testTransactionId).Result;

            //Assert
            Assert.IsNotNull(transaction);
            Assert.That(transaction, Is.InstanceOf(typeof(Transaction)));
            Assert.AreEqual(_testTransactionId, transaction.TransactionId);

            //Check that the GetAll method was called once
            _transactionReadRepositoryMock.Verify(t => t.GetAll(), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _transactionReadRepositoryMock = null;
            _transactionWriteRepositoryMock = null;
            _paypalClientServiceMock = null;
            _userServiceMock = null;
            _donationServiceMock = null;
            _waterpumpProjectServiceMock = null;
        }
    }
}