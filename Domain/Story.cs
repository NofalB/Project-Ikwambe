using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Story
    {

        [OpenApiProperty(Description = "get or sets the id of the story")]
        [JsonRequired]
        public Guid StoryId { get; set; }

        [OpenApiProperty(Description = "get or sets the title of the story")]
        [JsonRequired]
        public string Title { get; set; }

        [OpenApiProperty(Description = "get or sets the images of the story")]
        [JsonRequired]
        public List<StoryImage> StoryImages { get; set; }

        [OpenApiProperty(Description = "Get or sets the date when the story is published.")]
        [JsonRequired]
        public DateTime PublishDate { get; set; }
        
        [OpenApiProperty(Description = "Get or sets the summary of the story.")]
        [JsonRequired]
        public string Summary { get; set; }

        [OpenApiProperty(Description = "Get or sets the summary of the story.")]
        [JsonRequired]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Get or sets the author of the story.")]
        [JsonRequired]
        public string Author { get; set; }
        
        public string PartitionKey { get; set; }//we wait with front end/ author?
        //partition key genre water pump. education.

        public Story()
        {
            
        }

        public Story(Guid storyId, string title, DateTime publishDate, string summary, string description, string author)
        {
            StoryId = storyId;
            Title = title;
            PublishDate = publishDate;
            Summary = summary;
            Description = description;
            Author = author;
            PartitionKey = author;
        }
    }

    public class DummyStoryxample : OpenApiExample<Story>
    {
        public override IOpenApiExample<Story> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("story1", new Story() { StoryId = Guid.NewGuid(), 
                Title = "story of story1", 
                StoryImages = new List<StoryImage>()
                {
                    new StoryImage(/*"Image 1",*/ "owf4fzify7by.jpg"),
                    new StoryImage(/*"Image 2",*/ "22owf4fzify7by22.jpg"),
                }, 
                PublishDate = DateTime.Now, Summary = "this is the story", 
                Description = "this should be a long description", 
                Author = "stephen" }, NamingStrategy)
            );

            Examples.Add(OpenApiExampleResolver.Resolve("story2", new Story() { StoryId = Guid.NewGuid(), 
                Title = "story of story2", 
                StoryImages = new List<StoryImage>()
                {
                    new StoryImage(/*"Image 3",*/ "randomImage3.jpg"),
                    new StoryImage(/*"Image 4",*/ "randomImage4.jpg"),
                },
                PublishDate = DateTime.Now, Summary = "this is the second story", 
                Description = "this should be a long second description", 
                Author = "stephen" }, NamingStrategy)
            );

            return this;
        }
    }

    public class DummyStoryExamples : OpenApiExample<List<Story>>
    {

        public override IOpenApiExample<List<Story>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("stories", new List<Story>()
            {
                new Story () 
                { 
                    StoryId = Guid.NewGuid(), Title = "story of story1",
                    StoryImages = new List<StoryImage>()
                    {
                        new StoryImage(/*"Image 3",*/ "randomImage3.jpg"),
                        new StoryImage(/*"Image 4",*/ "randomImage4.jpg"),
                    }, 
                    PublishDate = DateTime.Now, Summary = "this is the story", 
                    Description = "this should be a long description", 
                    Author ="stephen" 
                },
                   
                new Story() 
                { 
                    StoryId = Guid.NewGuid(), Title = "story of story2",
                    StoryImages = new List<StoryImage>()
                    {
                        new StoryImage(/*"Image 3",*/ "randomImage3.jpg"),
                        new StoryImage(/*"Image 4",*/ "randomImage4.jpg"),
                    },
                    PublishDate = DateTime.Now, Summary = "this is the second story", 
                    Description = "this should be a long second description", 
                    Author ="stephen"
                }
            }));

            return this;
        }
    }
}
