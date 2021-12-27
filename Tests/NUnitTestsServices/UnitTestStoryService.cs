using Infrastructure.Repositories;
using Infrastructure.Services;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.DTO;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace NUnitTestsServices
{
    public class UnitTestStoryService
    {
        private StoryService _storyService;
        private List<Story> _mockListStories;
        private Mock<ICosmosReadRepository<Story>> _storyReadRepositoryMock;
        private Mock<ICosmosWriteRepository<Story>> _storyWriteRepositoryMock;
        private IOptions<BlobCredentialOptions> _options;

    
        [SetUp]
        public void Setup()
        {
            _storyReadRepositoryMock = new Mock<ICosmosReadRepository<Story>>();
            _storyWriteRepositoryMock = new Mock<ICosmosWriteRepository<Story>>();

            _storyService = new StoryService(_storyReadRepositoryMock.Object, _storyWriteRepositoryMock.Object);

            //set up mock data
            _mockListStories = new List<Story>()
            {
                new Story () { StoryId = Guid.NewGuid(), Title = "story of story1", StoryImages = new List<StoryImage>()
                {
                    new StoryImage("Image 1", "owf4fzify7by.jpg"),
                    new StoryImage("Image 2", "22owf4fzify7by22.jpg"),
                }, 
                PublishDate = DateTime.Now, Summary = "this is the story",  Description = "this should be a long description", Author ="stephen" },
                new Story() { StoryId = Guid.NewGuid(), Title = "story of story2", StoryImages = new List<StoryImage>()
                {
                    new StoryImage("Image 1", "owf4fzify7by.jpg"),
                    new StoryImage("Image 2","22owf4fzify7by22.jpg"),
                },  
                PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author ="stephen"}
            };

        }

        [Test]
        public void GetAllStories_Should_Return_Specific_MockStoryAsync()
        {
            //arrange
            var mockStory = _mockListStories.AsQueryable().BuildMockDbSet();
            _storyReadRepositoryMock.Setup(s => s.GetAll()).Returns(mockStory.Object);
            
            //act
            var stories = _storyService.GetAllStories().Result.ToList();

            //assert
            Assert.IsNotNull(stories);
            Assert.That(stories, Is.InstanceOf(typeof(IEnumerable<Story>)));
            Assert.AreEqual(2, stories.Count);

            _storyReadRepositoryMock.Verify(s => s.GetAll(), Times.Once);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _storyReadRepositoryMock = null;
            _storyWriteRepositoryMock = null;
            _mockListStories = null;
        }
    }
}
