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
        #region Mock List Data
        private List<Donation> _mockListDonations;
        private List<User> _mockListUsers;
        private List<Story> _mockListStories;
        private List<WaterpumpProject> _mockListWaterpumpProject;
        #endregion

        #region Write Repositories
        private Mock<ICosmosWriteRepository<Donation>> _donationWriteMock;
        private ICosmosWriteRepository<Donation> _donationWriteRepo;

        private Mock<ICosmosWriteRepository<User>> _userWriteMock;
        private ICosmosWriteRepository<User> _userWriteRepo;

        private Mock<ICosmosWriteRepository<Story>> _storyWriteMock;
        private ICosmosWriteRepository<Story> _storyWriteRepo;

        private Mock<ICosmosWriteRepository<WaterpumpProject>> _waterpumpProjectWriteMock;
        private ICosmosWriteRepository<WaterpumpProject> _waterpumpProjectWriteRepo;
        #endregion

        #region Read Repositories
        private Mock<ICosmosReadRepository<Donation>> _donationReadMock;
        private ICosmosReadRepository<Donation> _donationReadRepo;
       
        private Mock<ICosmosReadRepository<User>> _userReadMock;
        private ICosmosReadRepository<User> _userReadRepo;

        private Mock<ICosmosReadRepository<Story>> _storyReadMock;
        private ICosmosReadRepository<Story> _storyReadRepo;

        private Mock<ICosmosReadRepository<WaterpumpProject>> _waterpumpProjectReadMock;
        private ICosmosReadRepository<WaterpumpProject> _waterpumpProjectReadRepo;
        #endregion

        [SetUp]
        public void Setup()
        {
            // setup repositories
            SetupRepos();

            // setup mock data
            SetupMockData();

            // setup callback methods
            SetupCallbackMethods();
        }

        private void SetupRepos()
        {
            // donation repos
            _donationReadMock = new Mock<ICosmosReadRepository<Donation>>();
            _donationReadRepo = _donationReadMock.Object;
            _donationWriteMock = new Mock<ICosmosWriteRepository<Donation>>();
            _donationWriteRepo = _donationWriteMock.Object;

            // donation repos
            _userReadMock = new Mock<ICosmosReadRepository<User>>();
            _userReadRepo = _userReadMock.Object;
            _userWriteMock = new Mock<ICosmosWriteRepository<User>>();
            _userWriteRepo = _userWriteMock.Object;

            // donation repos
            _storyReadMock = new Mock<ICosmosReadRepository<Story>>();
            _storyReadRepo = _storyReadMock.Object;
            _storyWriteMock = new Mock<ICosmosWriteRepository<Story>>();
            _storyWriteRepo = _storyWriteMock.Object;

            // donation repos
            _waterpumpProjectReadMock = new Mock<ICosmosReadRepository<WaterpumpProject>>();
            _waterpumpProjectReadRepo = _waterpumpProjectReadMock.Object;
            _waterpumpProjectWriteMock = new Mock<ICosmosWriteRepository<WaterpumpProject>>();
            _waterpumpProjectWriteRepo = _waterpumpProjectWriteMock.Object;
        }

        private void SetupMockData()
        {
            // donations
            _mockListDonations = new List<Donation> {
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
            };
            _donationReadMock.Setup(m => m.GetAll()).Returns(_mockListDonations.AsQueryable());

            // users
            _mockListUsers = new List<User> {
                new User(Guid.NewGuid(), "Kratos", "Jumbo", "bruh@gmail.com", "380",true),
                new User(Guid.NewGuid(), "Bam", "Test", "bruh@gmail.com", "Hello123",true)
            };
            _userReadMock.Setup(m => m.GetAll()).Returns(_mockListUsers.AsQueryable());

            // stories
            _mockListStories = new List<Story> {
                new Story () { StoryId = Guid.NewGuid(), Title = "story of story1", ImageURL = "owf4fzify7by.jpg", PublishDate = DateTime.Now, Summary = "this is the story",  Description = "this should be a long description", Author ="stephen" },
                new Story() { StoryId = Guid.NewGuid(), Title = "story of story2", ImageURL = "randomImage.jpg", PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author ="stephen"}
            };
            _storyReadMock.Setup(m => m.GetAll()).Returns(_mockListStories.AsQueryable());

            // waterpumpProject
            _mockListWaterpumpProject = new List<WaterpumpProject> {
                new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPump Ikwambe",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20, ProjectType = ProjectType.infrastructure},
                new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAlmere",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50, ProjectType = ProjectType.infrastructure},
            };
            _waterpumpProjectReadMock.Setup(m => m.GetAll()).Returns(_mockListWaterpumpProject.AsQueryable());

        }

        private void SetupCallbackMethods()
        {
            // add donation
            _donationWriteMock.Setup(m => m.AddAsync(It.IsAny<Donation>())).Callback(new Action<Donation>(
                x =>
                {
                    _mockListDonations.Add(x);
                    _mockListDonations.AsQueryable();
                }
            ));

            // add user
            _userWriteMock.Setup(m => m.AddAsync(It.IsAny<User>())).Callback(new Action<User>(
                x =>
                {
                    _mockListUsers.Add(x);
                    _mockListUsers.AsQueryable();
                }
            ));

            // add story
            _storyWriteMock.Setup(m => m.AddAsync(It.IsAny<Story>())).Callback(new Action<Story>(
                x =>
                {
                    _mockListStories.Add(x);
                    _mockListStories.AsQueryable();
                }
            ));

            // add waterpumpProject
            _waterpumpProjectWriteMock.Setup(m => m.AddAsync(It.IsAny<WaterpumpProject>())).Callback(new Action<WaterpumpProject>(
                x =>
                {
                    _mockListWaterpumpProject.Add(x);
                    _mockListWaterpumpProject.AsQueryable();
                }
            ));
        }

        [Test]
        public void Insert_Donation_Should_Return_Increased_MockListDonations()
        {
            // Arrange
            Donation testDonation = new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 200);
            User testUser = new User(Guid.NewGuid(), "Jumbo", "Kratos", "bruh@gmail.com", "tEst12345", false);
            Story testStory = new Story() { StoryId = Guid.NewGuid(), Title = "story of story1", ImageURL = "owf4fzify7by.jpg", PublishDate = DateTime.Now, Summary = "this is the story", Description = "this should be a long description", Author = "hamza" };
            WaterpumpProject testWaterpumpProject = new WaterpumpProject()
            {
                ProjectId = Guid.NewGuid(),
                NameOfProject = "waterPumpAmsterdam",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330),
                CurrentTotal = 456,
                TargetGoal = 66000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                RatedPower = 50,
                FlowRate = 200,
                ProjectType = ProjectType.infrastructure
            };


            //Act
            _donationWriteRepo.AddAsync(testDonation);
            var afterAddingDonation = (IList<Donation>)_donationReadRepo.GetAll().ToList();

            _userWriteRepo.AddAsync(testUser);
            var afterAddingUser = (IList<Donation>)_donationReadRepo.GetAll().ToList();

            _storyWriteRepo.AddAsync(testStory);
            var afterAddingStory = (IList<Donation>)_donationReadRepo.GetAll().ToList();

            _waterpumpProjectWriteRepo.AddAsync(testWaterpumpProject);
            var afterAddingWaterpumpProject = (IList<Donation>)_donationReadRepo.GetAll().ToList();

            //Assert
            Assert.AreEqual(3, afterAddingDonation.Count);
            Assert.AreEqual(3, afterAddingUser.Count);
            Assert.AreEqual(3, afterAddingStory.Count);
            Assert.AreEqual(3, afterAddingWaterpumpProject.Count);
        }
    }
}