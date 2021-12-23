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

namespace Domain.DTO
{
    public class StoryDTO
    {
        public string Title { get; set; }

        [OpenApiProperty(Description = "get or sets the image URL")]
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
    }
    public class DummyStoryDTOExample : OpenApiExample<StoryDTO>
    {
        public override IOpenApiExample<StoryDTO> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("story1", new StoryDTO()
            {
                Title = "story of story1",
                StoryImages = new List<StoryImage>()
                {
                    new StoryImage(/*"Image 1",*/ "owf4fzify7by.jpg"),
                    new StoryImage(/*"Image 2",*/ "22owf4fzify7by22.jpg"),
                },
                PublishDate = DateTime.Now,
                Summary = "this is the story",
                Description = "this should be a long description",
                Author = "stephen"
            }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("story2", new StoryDTO()
            {
                Title = "story of story2",
                StoryImages = new List<StoryImage>()
                {
                    new StoryImage(/*"Image 1", */"owf4fzify7by.jpg"),
                    new StoryImage(/*"Image 2",*/ "22owf4fzify7by22.jpg"),
                },
                PublishDate = DateTime.Now,
                Summary = "this is the second story",
                Description = "this should be a long second description",
                Author = "stephen"
            }, NamingStrategy));

            return this;
        }
    }

    public class DummyStoryDTOExamples : OpenApiExample<List<StoryDTO>>
    {
        public override IOpenApiExample<List<StoryDTO>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("stories", new List<StoryDTO>()
                {
                   new StoryDTO() {
                       Title = "story of story1",
                       StoryImages = new List<StoryImage>()
                       {
                           new StoryImage(/*"Image 1",*/ "owf4fzify7by.jpg"),
                           new StoryImage(/*"Image 2",*/ "22owf4fzify7by22.jpg"),
                       },
                       PublishDate = DateTime.Now, Summary = "this is the story", Description = "this should be a long description", Author ="stephen" },
                   new StoryDTO() { Title = "story of story2",
                       StoryImages = new List<StoryImage>()
                       {
                           new StoryImage(/*"Image 1",*/ "owf4fzify7by.jpg"),
                           new StoryImage(/*"Image 2",*/ "22owf4fzify7by22.jpg"),
                       },
                       PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author ="stephen"}
                }));
            return this;
        }
    }

}

