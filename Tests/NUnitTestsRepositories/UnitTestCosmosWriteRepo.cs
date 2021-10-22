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
        private Guid _testId;

        #region Mock List Data
        private List<Donation> _mockListDonations;
        private List<User> _mockListUsers;
        private List<Story> _mockListStories;
        private List<WaterpumpProject> _mockListWaterpumpProject;
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

        [SetUp]
        public void Setup()
        {
            // setup test ID
            _testId = Guid.NewGuid();

            // setup repositories
            SetupRepos();

            // setup mock data
            SetupMockData();

            // setup callback methods
            SetupCallbackInsertMethods();
            SetupCallbackUpdateMethods();
            SetupCallbackDeleteMethods();
        }

        #region Setup Methods
        private void SetupRepos()
        {
            // donation repos
            _donationReadMock = new Mock<ICosmosReadRepository<Donation>>();
            _donationReadRepo = _donationReadMock.Object;
            _donationWriteMock = new Mock<ICosmosWriteRepository<Donation>>();
            _donationWriteRepo = _donationWriteMock.Object;

            // user repos
            _userReadMock = new Mock<ICosmosReadRepository<User>>();
            _userReadRepo = _userReadMock.Object;
            _userWriteMock = new Mock<ICosmosWriteRepository<User>>();
            _userWriteRepo = _userWriteMock.Object;

            // story repos
            _storyReadMock = new Mock<ICosmosReadRepository<Story>>();
            _storyReadRepo = _storyReadMock.Object;
            _storyWriteMock = new Mock<ICosmosWriteRepository<Story>>();
            _storyWriteRepo = _storyWriteMock.Object;

            // waterpumpProject repos
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
                new User(_testId, "Kratos", "Jumbo", "bruh@gmail.com", "380",true),
                new User(Guid.NewGuid(), "Bam", "Test", "bruh@gmail.com", "Hello123",true)
            };
            _userReadMock.Setup(m => m.GetAll()).Returns(_mockListUsers.AsQueryable());

            // stories
            _mockListStories = new List<Story> {
                new Story () { StoryId = _testId, Title = "story of story1", ImageURL = "owf4fzify7by.jpg", PublishDate = DateTime.Now, Summary = "this is the story",  Description = "this should be a long description", Author ="stephen" },
                new Story() { StoryId = Guid.NewGuid(), Title = "story of story2", ImageURL = "randomImage.jpg", PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author ="stephen"}
            };
            _storyReadMock.Setup(m => m.GetAll()).Returns(_mockListStories.AsQueryable());

            // waterpumpProject
            _mockListWaterpumpProject = new List<WaterpumpProject> {
                new WaterpumpProject() { ProjectId = _testId, NameOfProject = "waterPump Ikwambe",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20, ProjectType = ProjectType.infrastructure},
                new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAlmere",
                Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50, ProjectType = ProjectType.infrastructure},
            };
            _waterpumpProjectReadMock.Setup(m => m.GetAll()).Returns(_mockListWaterpumpProject.AsQueryable());

        }

        private void SetupCallbackInsertMethods()
        {
            // insert donation
            _donationWriteMock.Setup(m => m.AddAsync(It.IsAny<Donation>())).Callback(new Action<Donation>(
                x =>
                {
                    _mockListDonations.Add(x);
                    _mockListDonations.AsQueryable();
                }
            ));

            // insert user
            _userWriteMock.Setup(m => m.AddAsync(It.IsAny<User>())).Callback(new Action<User>(
                x =>
                {
                    _mockListUsers.Add(x);
                    _mockListUsers.AsQueryable();
                }
            ));

            // insert story
            _storyWriteMock.Setup(m => m.AddAsync(It.IsAny<Story>())).Callback(new Action<Story>(
                x =>
                {
                    _mockListStories.Add(x);
                    _mockListStories.AsQueryable();
                }
            ));

            // insert waterpumpProject
            _waterpumpProjectWriteMock.Setup(m => m.AddAsync(It.IsAny<WaterpumpProject>())).Callback(new Action<WaterpumpProject>(
                x =>
                {
                    _mockListWaterpumpProject.Add(x);
                    _mockListWaterpumpProject.AsQueryable();
                }
            ));
        }

        private void SetupCallbackUpdateMethods()
        {
             //update user
            _userWriteMock.Setup(x => x.Update(It.IsAny<User>())).Callback(new Action<User>(x =>
            {

                var userFound = _mockListUsers.Find(c => c.UserId == x.UserId);
                _mockListUsers.Remove(userFound);
                _mockListUsers.Add(x);
            }));

            //update story
            _storyWriteMock.Setup(x => x.Update(It.IsAny<Story>())).Callback(new Action<Story>(x =>
            {

                var storyFound = _mockListStories.Find(c => c.StoryId == x.StoryId);
                _mockListStories.Remove(storyFound);
                _mockListStories.Add(x);
            }));

            //update waterpumpProject
            _waterpumpProjectWriteMock.Setup(x => x.Update(It.IsAny<WaterpumpProject>())).Callback(new Action<WaterpumpProject>(x =>
            {

                var waterpumpProjectFound = _mockListWaterpumpProject.Find(c => c.ProjectId == x.ProjectId);
                _mockListWaterpumpProject.Remove(waterpumpProjectFound);
                _mockListWaterpumpProject.Add(x);
            }));
        }

        private void SetupCallbackDeleteMethods()
        {
            //delete User
            _userWriteMock.Setup(x => x.Delete(It.IsAny<User>())).Callback(new Action<User>(x =>
            {
                _mockListUsers.Remove(x);
            }));

            //delete story
            _storyWriteMock.Setup(x => x.Delete(It.IsAny<Story>())).Callback(new Action<Story>(x =>
            {
                _mockListStories.Remove(x);
            }));

            //delete waterpumpProject
            _waterpumpProjectWriteMock.Setup(x => x.Delete(It.IsAny<WaterpumpProject>())).Callback(new Action<WaterpumpProject>(x =>
            {
                _mockListWaterpumpProject.Remove(x);
            }));
        }
        #endregion

        [Test]
        public void Insert_Should_Return_Increased_Entities_MockListsData()
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
            var afterAddingUser = (IList<User>)_userReadRepo.GetAll().ToList();

            _storyWriteRepo.AddAsync(testStory);
            var afterAddingStory = (IList<Story>)_storyReadRepo.GetAll().ToList();

            _waterpumpProjectWriteRepo.AddAsync(testWaterpumpProject);
            var afterAddingWaterpumpProject = (IList<WaterpumpProject>)_waterpumpProjectReadRepo.GetAll().ToList();

            //Assert
            Assert.AreEqual(3, afterAddingDonation.Count);
            Assert.AreEqual(3, afterAddingUser.Count);
            Assert.AreEqual(3, afterAddingStory.Count);
            Assert.AreEqual(3, afterAddingWaterpumpProject.Count);
        }

        [Test]
        public void Update_Should_Edit_Entity_Data()
        {
            // Arrange
            User testUser = new User(_testId, "Jumbo2", "Kratos3", "bruh@gmail.com4", "tEst123455", false);
            Story testStory = new Story() { StoryId = _testId, Title = "new story of story1 v2", ImageURL = "owf4fzify7by.jpg", PublishDate = DateTime.Now, Summary = "this is the story", Description = "this should be a long description", Author = "hamza" };
            WaterpumpProject testWaterpumpProject = new WaterpumpProject()
            {
                ProjectId = _testId,
                NameOfProject = "waterPumpAmsterdam v2",
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
            _userWriteRepo.Update(testUser);
            _storyWriteRepo.Update(testStory);
            _waterpumpProjectWriteRepo.Update(testWaterpumpProject);

            //Assert
            Assert.AreEqual("Jumbo2", _mockListUsers.Find(u => u.UserId == testUser.UserId).FirstName);
            Assert.AreEqual("new story of story1 v2", _mockListStories.Find(s => s.StoryId == testStory.StoryId).Title);
            Assert.AreEqual("waterPumpAmsterdam v2", _mockListWaterpumpProject
                .Find(w => w.ProjectId == testWaterpumpProject.ProjectId).NameOfProject);
        }

        [Test]
        public void Delete_Should_Return_Decreased_MockListsData()
        {
            // Arrange
            var nrOfUsers = _mockListUsers.Count();
            User testUser = _mockListUsers.First(u => u.UserId == _testId);

            var nrOfStories = _mockListStories.Count();
            Story testStory = _mockListStories.First(s => s.StoryId == _testId);

            var nrOfWaterpumpProject= _mockListUsers.Count();
            WaterpumpProject testWaterpumpProject = _mockListWaterpumpProject.First(w => w.ProjectId == _testId);

            //Act
            _userWriteRepo.Delete(testUser);
            _storyWriteRepo.Delete(testStory);
            _waterpumpProjectWriteRepo.Delete(testWaterpumpProject);

            //Assert
            Assert.AreEqual(_mockListUsers.Count, nrOfUsers-1);
            Assert.AreEqual(_mockListStories.Count, nrOfStories-1);
            Assert.AreEqual(_mockListWaterpumpProject.Count, nrOfWaterpumpProject-1);
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

            _donationWriteMock = null;
            _userWriteMock = null;
            _storyWriteMock = null;
            _waterpumpProjectWriteMock = null;

            _donationWriteRepo = null;
            _userWriteRepo = null;
            _storyWriteRepo = null;
            _waterpumpProjectWriteRepo = null;

            _mockListDonations = null;
            _mockListUsers = null;
            _mockListStories = null;
            _mockListWaterpumpProject= null;
        }
    }
}