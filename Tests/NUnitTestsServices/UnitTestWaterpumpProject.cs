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


namespace NUnitTestsServices
{
    public class UnitTestWaterpumpProject
    {
        private WaterpumpProjectService _waterpumpProjectSevice;
        private IEnumerable<WaterpumpProject> _mockListWaterpumpProjects;
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
                    new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPump Ikwambe",
                    Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20, ProjectType = ProjectType.infrastructure},
                    new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAlmere",
                    Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50, ProjectType = ProjectType.infrastructure},
                    new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAmsterdam",
                    Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 456, TargetGoal = 66000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 50, FlowRate = 200, ProjectType = ProjectType.infrastructure}
                });
        }

        [Test]
        public async Task GetAllWaterpumpProjects_should_return_all_MockWaterpumpProjectAsync()
        {
            //Arrange
            _waterpumpProjectReadRepositoryMock.Setup(w => w.GetAll()).Returns((IQueryable<WaterpumpProject>)(List<WaterpumpProject>)_mockListWaterpumpProjects);

            //Act
            IEnumerable<WaterpumpProject> waterpumpProjects = await _waterpumpProjectSevice.GetAllWaterpumpProjectsAsync();

            //assert
            Assert.IsNotNull(waterpumpProjects);
            Assert.That(waterpumpProjects, Is.InstanceOf(typeof(IEnumerable<WaterpumpProject>)));

            //check if get all is called
            _waterpumpProjectReadRepositoryMock.Verify(w => w.GetAll(), Times.Once);
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
