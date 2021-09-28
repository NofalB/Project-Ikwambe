using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProjectIkwambe.Models
{
	[OpenApiExample(typeof(LoginRequestExample))]
	public class LoginRequest
    {
		[OpenApiProperty(Description = "Username for the user logging in.")]
		[JsonRequired]
		public string Username { get; set; }

		[OpenApiProperty(Description = "Password for the user logging in.")]
		[JsonRequired]
		public string Password { get; set; }
	}

	public class LoginRequestExample : OpenApiExample<LoginRequest>
	{
		public override IOpenApiExample<LoginRequest> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Stephen",
														new LoginRequest()
														{
															Username = "Stephen",
															Password = "SuperSecretPassword123!!"
														},
														NamingStrategy));

			return this;
		}
	}

}
