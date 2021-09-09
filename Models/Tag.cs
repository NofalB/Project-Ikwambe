using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Petstore.Models {
	/// <summary>
	/// This represents the model entity for tag of Swagger Pet Store.
	/// </summary>
	public class Tag {
		[OpenApiProperty(Description = "Gets or sets the tag ID.")]
		public long? Id { get; set; }

		[OpenApiProperty(Description = "Gets or sets the name..")]
		public string Name { get; set; }
	}
}