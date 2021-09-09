using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Petstore.Models {
	//[OpenApiProperty(Description = "This represents the model entity for pet of Swagger Pet Store.")]

	[OpenApiExample(typeof(DummyPetExample))]
	public class Pet {
		[OpenApiProperty(Description = "Gets or sets the pet ID.")]
		public long? Id { get; set; }

		[OpenApiProperty(Description = "Gets or sets the category.")]
		public Category Category { get; set; }

		[OpenApiProperty(Description = "Gets or sets the name.")]
		[JsonRequired]
		public string Name { get; set; }

		[OpenApiProperty(Description = "Gets or sets the list of photo URLs.")]
		[JsonRequired]
		public List<string> PhotoUrls { get; set; } = new List<string>();

		[OpenApiProperty(Description = "Gets or sets the list of tags.")]
		public List<Tag> Tags { get; set; }

		[OpenApiProperty(Description = "Gets or sets the PetStatus value.")]
		[JsonRequired]
		public PetStatus Status { get; set; }
	}

	public class DummyPetExample : OpenApiExample<Pet> {
		public override IOpenApiExample<Pet> Build(NamingStrategy NamingStrategy = null) {
			Examples.Add(OpenApiExampleResolver.Resolve("Wagner", new Pet() { Id = 33, Name = "Wagner", Status = PetStatus.Available }, NamingStrategy));
			Examples.Add(OpenApiExampleResolver.Resolve("Zeus", "This is Zeus' summary", new Pet() { Id = 34, Name = "Zeus", Status = PetStatus.Pending }, NamingStrategy));
			Examples.Add(OpenApiExampleResolver.Resolve("Bruno", "This is bruno's summary", "This is bruno's description", new Pet() { Id = 35, Name = "Bruno", Status = PetStatus.Sold }, NamingStrategy));

			return this;
		}
	}

	public class DummyPetExamples : OpenApiExample<List<Pet>> {
		public override IOpenApiExample<List<Pet>> Build(NamingStrategy NamingStrategy = null) {
			Examples.Add(OpenApiExampleResolver.Resolve("Pets", new List<Pet> {
				new Pet() { Id = 33, Name = "Wagner", Status = PetStatus.Available },
				new Pet() { Id = 34, Name = "Zeus", Status = PetStatus.Pending },
				new Pet() { Id = 35, Name = "Bruno", Status = PetStatus.Sold },
			}));

			return this;
		}
	}
}
