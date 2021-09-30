using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.DTO
{
	[OpenApiExample(typeof(LoginRequestExample))]
	public class LoginRequest
    {
		[OpenApiProperty(Description = "Email for the user logging in.")]
		[JsonRequired]
		public string Email { get; set; }

		[OpenApiProperty(Description = "Password for the user logging in.")]
		[JsonRequired]
		public string Password { get; set; }
	}
	public class LoginRequestExample : OpenApiExample<LoginRequest>
	{
		public override IOpenApiExample<LoginRequest> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Stephen", new LoginRequest() { Email = "Stephen@gmail.com", Password = "SuperSecretPassword123!!"}, NamingStrategy));
			return this;
		}
	}
}
