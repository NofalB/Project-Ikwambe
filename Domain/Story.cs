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
        public long? Id { get; set; }

        [OpenApiProperty(Description = "get or sets the image URL")]
        [JsonRequired]
        public string imageURL { get; set; }

        [OpenApiProperty(Description = "Get or sets the date when the story is published.")]
        [JsonRequired]
        public DateTime? publishDate { get; set; }
        
        [OpenApiProperty(Description = "Get or sets the summary of the story.")]
        [JsonRequired]
        public string summary { get; set; }

        [OpenApiProperty(Description = "Get or sets the summary of the story.")]
        [JsonRequired]
        public string description { get; set; }

    }

    public class DummyPetExample : OpenApiExample<Story>
    {
        public override IOpenApiExample<Story> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("story1", new Story() { Id = 1, imageURL = "owf4fzify7by.jpg", publishDate = DateTime.Now, summary = "this is the story", description = "this should be a long description" }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("story2", new Story() { Id = 2, imageURL = "randomImage.jpg", publishDate = DateTime.Now, summary = "this is the second story", description = "this should be a long second description" }, NamingStrategy));

            return this;
        }
    }

    public class DummyStoryExamples : OpenApiExample<List<Story>>
    {

        public override IOpenApiExample<List<Story>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("stories", new List<Story>()
                {
                   new Story () { Id = 1, imageURL = "owf4fzify7by.jpg", publishDate = DateTime.Now, summary = "this is the story",  description = "this should be a long description" },
                   new Story() { Id = 2, imageURL = "randomImage.jpg", publishDate = DateTime.Now, summary = "this is the second story", description = "this should be a long second description"}
                }));

            return this;
        }
    }
}
