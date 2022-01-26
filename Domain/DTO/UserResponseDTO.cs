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
	public class UserResponseDTO : User
    {
		public UserResponseDTO(Guid id, string firstName, string lastName, bool subscription)
		{
			base.UserId = id;
			base.FirstName = firstName;
			base.LastName = lastName;
			base.Subscription = subscription;
		}
	}
}
