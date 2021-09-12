using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PetStore.Models
{
    class User
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

        public User()
        {

        }


		public class DummyUserExample : OpenApiExample<User>
		{
			public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("Hamza", new User(100, "Kratos", "Mumbo", "kratos@gmail.com", "420"), NamingStrategy));
				Examples.Add(OpenApiExampleResolver.Resolve("Bruno", new User(101, "Bam", "test", "kratos@gmail.com", "420"), NamingStrategy));
				Examples.Add(OpenApiExampleResolver.Resolve("Jumbo", new User(102, "Jumbo", "testing", "kratos@gmail.com", "420"), NamingStrategy));

				return this;
			}
		}

		public class DummyUserExamples : OpenApiExample<List<User>>
		{
			public override IOpenApiExample<List<User>> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("Users", new List<User> {
				new User(100, "Kratos", "Mumbo", "kratos@gmail.com", "420"),
				new User(101, "Bam", "test", "kratos@gmail.com", "420"),
				new User(102, "Jumbo", "testing", "kratos@gmail.com", "420"),
			}));

				return this;
			}
		}
	}
}
