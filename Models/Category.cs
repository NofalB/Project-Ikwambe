using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace ProjectIkwambe.Models {
	/// <summary>
	/// This represents the model entity for category of Swagger Pet Store.
	/// </summary>
	public class Category {
		[OpenApiProperty(Description = "Gets or sets the category ID.")] 
		public long? Id { get; set; }

		[OpenApiProperty(Description = "Gets or sets the name.")]
		public string Name { get; set; }
	}
}
