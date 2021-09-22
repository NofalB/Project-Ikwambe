using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace ProjectIkwambe.Configurations {
	public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions {
		public override OpenApiInfo Info { get; set; } = new OpenApiInfo() {
			Version = "3.0.0",
			Title = "PetStore example for .NET 5",
			Description = "An example of how to set up a .NET 5 Azure Functions project with OpenAPI support.",
			TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
			Contact = new OpenApiContact() {
				Name = "Erwin de Vries",
				Email = "erwin.devries@inholland.nl",
				//Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
			},
			License = new OpenApiLicense() {
				Name = "MIT",
				Url = new Uri("http://opensource.org/licenses/MIT"),
			}
		};

		public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
	}
}
