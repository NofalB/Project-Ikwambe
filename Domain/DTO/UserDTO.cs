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
    public class UserDTO
    {
		[OpenApiProperty(Description = "Gets or sets the first name.")]
		[JsonRequired]
		public string FirstName { get; set; }

		[OpenApiProperty(Description = "Gets or sets the last name.")]
		[JsonRequired]
		public string LastName { get; set; }

		[OpenApiProperty(Description = "Gets or sets the email.")]
		[JsonRequired]
		public string Email { get; set; }

		[OpenApiProperty(Description = "Gets or sets the password.")]
		[JsonRequired]
		public string Password { get; set; }

		[OpenApiProperty(Description = "Gets or sets the subscription status.")]
		[JsonRequired]
		public bool Subscription { get; set; }

		/*public Role Role { get; set; }*/

		public UserDTO(string firstName, string lastName, string email, string password, bool subscription /*Role role*/)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Password = password;
			Subscription = subscription;
			//Role = role;
		}

		public UserDTO()
		{

		}
	}
	public class DummyUserDTOExample : OpenApiExample<UserDTO>
	{
		public override IOpenApiExample<UserDTO> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Hamza", new UserDTO("Kratos", "Jumbo", "bruh@gmail.com", "380", true /*Role.Admin*/), NamingStrategy));
			Examples.Add(OpenApiExampleResolver.Resolve("Bruno", new UserDTO("Bam", "Test", "bruh@gmail.com", "Hello123", true /*Role.Admin*/), NamingStrategy));
			Examples.Add(OpenApiExampleResolver.Resolve("Jumbo", new UserDTO("Jumbo", "Kratos", "bruh@gmail.com", "tEst12345", false /*Role.User*/), NamingStrategy));

			return this;
		}
	}

	public class DummyUserDTOExamples : OpenApiExample<List<UserDTO>>
	{
		public override IOpenApiExample<List<UserDTO>> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Users", new List<UserDTO> {
				new UserDTO("Kratos", "Jumbo", "bruh@gmail.com", "380", true /*Role.Admin*/),
				new UserDTO("Bam", "Test", "bruh@gmail.com", "Hello123", true /*Role.Admin*/),
				new UserDTO("Jumbo", "Kratos", "bruh@gmail.com", "tEst12345",true /*Role.Admin*/),
			}));

			return this;
		}
	}
}
