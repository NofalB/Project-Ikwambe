using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Services;
using Infrastructure.Repositories;
using NUnit.Framework;
using Moq;
using MockQueryable.Moq;

namespace NUnitTestsServices
{
    public class UnitTestUserService
    {
        private UserService _userService;
        private List<User> _mockListUsers;
        private Mock<ICosmosReadRepository<User>> _userReadRepositoryMock;
        private Mock<ICosmosWriteRepository<User>> _userWriteRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _userReadRepositoryMock = new Mock<ICosmosReadRepository<User>>();
            _userWriteRepositoryMock = new Mock<ICosmosWriteRepository<User>>();

            _userService = new UserService(_userReadRepositoryMock.Object, _userWriteRepositoryMock.Object);

            _mockListUsers = (
                new List<User> {
                    new User(Guid.NewGuid(), "Kratos", "Jumbo", "bruh@gmail.com", "380", true),
                    new User(Guid.NewGuid(), "Bam", "Test", "bruh@gmail.com", "Hello123", true),
                    new User(Guid.NewGuid(), "Jumbo", "Kratos", "bruh@gmail.com", "tEst12345", false)
                });
        }

        [Test]
        public void GetAllUser_Should_Return_All_MockUserAsync()
        {
            //Arrange
            var mockUsers = _mockListUsers.AsQueryable().BuildMockDbSet();
            _userReadRepositoryMock.Setup(s => s.GetAll()).Returns(mockUsers.Object);

            //Act
            var users = _userService.GetAllUsers().Result.ToList();

            //Assert
            Assert.IsNotNull(users);
            Assert.That(users, Is.InstanceOf(typeof(IEnumerable<User>)));
            Assert.AreEqual(3, users.Count);

            //check if get all is called
            _userReadRepositoryMock.Verify(s => s.GetAll(), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _userReadRepositoryMock = null;
            _userWriteRepositoryMock = null;
            _mockListUsers = null;
        }
    }
}
