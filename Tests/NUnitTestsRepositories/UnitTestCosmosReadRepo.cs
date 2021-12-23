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
        // mock data list
        private List<Donation> _mockListDonations;
        private List<User> _mockListUsers;
        private List<Story> _mockListStories;
        private List<WaterpumpProject> _mockListWaterpumpProject;

        // repos
        private Mock<ICosmosReadRepository<Donation>> _donationReadMock;
        private ICosmosReadRepository<Donation> _donationReadRepo;

        private Mock<ICosmosReadRepository<User>> _userReadMock;
        private ICosmosReadRepository<User> _userReadRepo;

        private Mock<ICosmosReadRepository<Story>> _storyReadMock;
        private ICosmosReadRepository<Story> _storyReadRepo;

        private Mock<ICosmosReadRepository<WaterpumpProject>> _waterpumpProjectReadMock;
        private ICosmosReadRepository<WaterpumpProject> _waterpumpProjectReadRepo;

        [SetUp]
        public void Setup()
        {
            // setup mock repos
            _donationReadMock = new Mock<ICosmosReadRepository<Donation>>();
            _donationReadRepo = _donationReadMock.Object;
            
            _userReadMock = new Mock<ICosmosReadRepository<User>>();
            _userReadRepo = _userReadMock.Object;
            
            _storyReadMock = new Mock<ICosmosReadRepository<Story>>();
            _storyReadRepo = _storyReadMock.Object;
            
            _waterpumpProjectReadMock = new Mock<ICosmosReadRepository<WaterpumpProject>>();
            _waterpumpProjectReadRepo = _waterpumpProjectReadMock.Object;

            // setup mock data
            SetupMockData();
        }

        private void SetupMockData()
        {
            _mockListDonations = new List<Donation> {
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625W", 599)
            };

            _mockListUsers = new List<User> {
                new User(Guid.NewGuid(), "Kratos", "Jumbo", "bruh@gmail.com", "380",true),
                new User(Guid.NewGuid(), "Bam", "Test", "bruh@gmail.com", "Hello123",true)
            };

            _mockListStories = new List<Story> {
                new Story () { StoryId = Guid.NewGuid(), Title = "story of story1", StoryImages = new List<StoryImage>()
                {
                    new StoryImage("Image 1", "owf4fzify7by.jpg"),
                    new StoryImage("Image 2", "22owf4fzify7by22.jpg"),
                }, 
                PublishDate = DateTime.Now, Summary = "this is the story",  Description = "this should be a long description", Author ="stephen" },
                new Story() { StoryId = Guid.NewGuid(), Title = "story of story2", StoryImages = new List<StoryImage>()
                {
                    new StoryImage("Image 1", "owf4fzify7by.jpg"),
                    new StoryImage("Image 2", "22owf4fzify7by22.jpg"),
                }, 
                PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author ="stephen"}
            };

            _mockListWaterpumpProject = new List<WaterpumpProject> {
                new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPump Ikwambe",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20, ProjectType = ProjectType.infrastructure},
                new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAlmere",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50, ProjectType = ProjectType.infrastructure},
            };
        }

        [Test]
        public void GetAll_Should_Return_All_MockListData()
        {
            //Get all mock data
            _donationReadMock.Setup(m => m.GetAll()).Returns(_mockListDonations.AsQueryable());
            _userReadMock.Setup(m => m.GetAll()).Returns(_mockListUsers.AsQueryable());
            _storyReadMock.Setup(m => m.GetAll()).Returns(_mockListStories.AsQueryable());
            _waterpumpProjectReadMock.Setup(m => m.GetAll()).Returns(_mockListWaterpumpProject.AsQueryable());

            //Act
            var donations = (IList<Donation>)_donationReadRepo.GetAll().ToList();
            var users = (IList<User>)_userReadRepo.GetAll().ToList();
            var stories = (IList<Story>)_storyReadRepo.GetAll().ToList();
            var waterpumpProjectList = (IList<WaterpumpProject>)_waterpumpProjectReadRepo.GetAll().ToList();

            //Assert
            Assert.AreEqual(2, donations.Count);
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(2, stories.Count);
            Assert.AreEqual(2, waterpumpProjectList.Count);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _donationReadMock = null;
            _userReadMock = null;
            _storyReadMock = null;
            _waterpumpProjectReadMock = null;

            _donationReadRepo = null;
            _userReadRepo = null;
            _storyReadRepo = null;
            _waterpumpProjectReadRepo = null;

            _mockListDonations = null;
            _mockListUsers = null;
            _mockListStories = null;
            _mockListWaterpumpProject = null;
        }
    }
}