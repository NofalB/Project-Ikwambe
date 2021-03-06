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
	public class User
	{
		[OpenApiProperty(Description = "Gets or sets the user ID.")]
		public Guid UserId { get; set; }

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

		[JsonIgnore]
		public Role Role { get; set; }

		public string PartitionKey { get; set; }//Subcription.
		

		public User(Guid userId, string firstName, string lastName, string email, string password,bool subscription)
		{
			UserId = userId;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Password = password;
			Subscription = subscription;
			//PartitionKey = subscription.ToString();
			//Role = Role.User;
		}

        public User()
        {

        }
	}

	public class DummyUserExample : OpenApiExample<User>
	{
		public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Hamza", new User(Guid.NewGuid(), "Kratos", "Jumbo", "bruh@gmail.com", "380", true), NamingStrategy));
			Examples.Add(OpenApiExampleResolver.Resolve("Bruno", new User(Guid.NewGuid(), "Bam", "Test", "bruh@gmail.com", "Hello123", true), NamingStrategy));
			Examples.Add(OpenApiExampleResolver.Resolve("Jumbo", new User(Guid.NewGuid(), "Jumbo", "Kratos", "bruh@gmail.com", "tEst12345", false), NamingStrategy));

			return this;
		}
	}

	public class DummyUserExamples : OpenApiExample<List<User>>
	{
		public override IOpenApiExample<List<User>> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Users", new List<User> {
				new User(Guid.NewGuid(), "Kratos", "Jumbo", "bruh@gmail.com", "380",true),
				new User(Guid.NewGuid(), "Bam", "Test", "bruh@gmail.com", "Hello123",true),
				new User(Guid.NewGuid(), "Jumbo", "Kratos", "bruh@gmail.com", "tEst12345",false),
			}));

			return this;
		}
	}
}
