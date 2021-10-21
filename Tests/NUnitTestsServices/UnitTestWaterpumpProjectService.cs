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
    public class UnitTestWaterpumpProjectService
    {
        private WaterpumpProjectService _waterpumpProjectSevice;
        private List<WaterpumpProject> _mockListWaterpumpProjects;
        private Mock<ICosmosReadRepository<WaterpumpProject>> _waterpumpProjectReadRepositoryMock;
        private Mock<ICosmosWriteRepository<WaterpumpProject>> _waterpumpProjectWriteRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _waterpumpProjectReadRepositoryMock = new Mock<ICosmosReadRepository<WaterpumpProject>>();
            _waterpumpProjectWriteRepositoryMock = new Mock<ICosmosWriteRepository<WaterpumpProject>>();


            _waterpumpProjectSevice = new WaterpumpProjectService(_waterpumpProjectReadRepositoryMock.Object, _waterpumpProjectWriteRepositoryMock.Object);

            _mockListWaterpumpProjects = (
                new List<WaterpumpProject> {
                    new WaterpumpProject() { ProjectId = Guid.Parse("f4019522-fa64-4052-87d6-9a6cc52081df"), NameOfProject = "waterPump Ikwambe",
                    Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20, ProjectType = ProjectType.infrastructure},
                    new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAlmere",
                    Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50, ProjectType = ProjectType.infrastructure},
                    new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAmsterdam",
                    Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 456, TargetGoal = 66000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 50, FlowRate = 200, ProjectType = ProjectType.infrastructure}
                });
        }

        [Test]
        public void GetAllWaterpumpProjects_should_return_all_MockWaterpumpProjectAsync()
        {
            //Arrange
            var mockWaterpumpProjects = _mockListWaterpumpProjects.AsQueryable().BuildMockDbSet();
            _waterpumpProjectReadRepositoryMock.Setup(w => w.GetAll()).Returns(mockWaterpumpProjects.Object);

            //Act
            var waterpumpProjects = _waterpumpProjectSevice.GetAllWaterpumpProjectsAsync().Result.ToList();

            //assert
            Assert.IsNotNull(waterpumpProjects);
            Assert.That(waterpumpProjects, Is.InstanceOf(typeof(IEnumerable<WaterpumpProject>)));
            Assert.AreEqual(3, waterpumpProjects.Count);

            //check if get all is called
            _waterpumpProjectReadRepositoryMock.Verify(w => w.GetAll(), Times.Once);
            //return Task.CompletedTask;
        }

        [Test]
        public void GetWaterpumpById_should_return_one_MockWaterpumpProjectAsync()
        {
            string projectId = "f4019522-fa64-4052-87d6-9a6cc52081df";
            //Arrange
            var mockWaterpumpProjects = _mockListWaterpumpProjects.AsQueryable().BuildMockDbSet();
            _waterpumpProjectReadRepositoryMock.Setup(w => w.GetAll().Where(w=>w.ProjectId == Guid.Parse(projectId)))
                .Returns(mockWaterpumpProjects.Object);
            //act
            var waterpumpProjects = _waterpumpProjectSevice.GetWaterPumpProjectById(projectId).Result;

            //assert
            Assert.IsNotNull(waterpumpProjects);
            Assert.That(waterpumpProjects, Is.InstanceOf(typeof(IEnumerable<WaterpumpProject>)));
            //Assert.AreEqual(1, waterpumpProjects.Count);

            //_waterpumpProjectReadRepositoryMock.Verify(w=>w.)
        }

        [TearDown]
        public void TestCleanUp()
        {
            _waterpumpProjectReadRepositoryMock = null;
            _waterpumpProjectWriteRepositoryMock = null;
            _mockListWaterpumpProjects = null;
        }
    }
}
