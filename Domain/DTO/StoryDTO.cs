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
        [OpenApiProperty(Description = "get or sets the image URL")]
        [JsonRequired]
        public string ImageURL { get; set; }

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
            Examples.Add(OpenApiExampleResolver.Resolve("story1", new StoryDTO() { ImageURL = "owf4fzify7by.jpg", PublishDate = DateTime.Now, Summary = "this is the story", Description = "this should be a long description", Author = "stephen" }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("story2", new StoryDTO() { ImageURL = "randomImage.jpg", PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author = "stephen" }, NamingStrategy));

            return this;
        }
    }

    public class DummyStoryDTOExamples : OpenApiExample<List<StoryDTO>>
    {
        public override IOpenApiExample<List<StoryDTO>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("stories", new List<StoryDTO>()
                {
                   new StoryDTO() { ImageURL = "owf4fzify7by.jpg", PublishDate = DateTime.Now, Summary = "this is the story", Description = "this should be a long description", Author ="stephen" },
                   new StoryDTO() { ImageURL = "randomImage.jpg", PublishDate = DateTime.Now, Summary = "this is the second story", Description = "this should be a long second description", Author ="stephen"}
                }));

            return this;
        }
    }

}
