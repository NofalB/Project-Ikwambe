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

namespace PetStore.Models
{
	[OpenApiExample(typeof(DummyUserExample))]
	public class User
	{
		[OpenApiProperty(Description = "Gets or sets the user ID.")]
		public long? Id { get; set; }

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

		public User(int id, string firstName, string lastName, string email, string password)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Password = password;
		}


		public class DummyUserExample : OpenApiExample<User>
		{
			public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("Hamza", new User(100, "Kratos", "Jumbo", "bruh@gmail.com", "420"), NamingStrategy));
				Examples.Add(OpenApiExampleResolver.Resolve("Bruno", new User(101, "Bam", "Test", "bruh@gmail.com", "420"), NamingStrategy));
				Examples.Add(OpenApiExampleResolver.Resolve("Jumbo", new User(102, "Jumbo", "Kratos", "bruh@gmail.com", "420"), NamingStrategy));

				return this;
			}
		}

		public class DummyUserExamples : OpenApiExample<List<User>>
		{
			public override IOpenApiExample<List<User>> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("Pets", new List<User> {
				new User(100,"Kratos","Jumbo","bruh@gmail.com","420"),
				new User(100, "Bam", "Test", "bruh@gmail.com", "420"),
				new User(100, "Jumbo", "Kratos", "bruh@gmail.com", "420"),
			}));

				return this;
			}
		}
	}
}
