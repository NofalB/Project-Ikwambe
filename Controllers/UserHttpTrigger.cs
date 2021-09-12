using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Petstore.Models;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;
using PetStore.Models;

namespace PetStore.Controllers
{
    class UserHttpTrigger
    {
		ILogger Logger { get; }

		public UserHttpTrigger(ILogger<UserHttpTrigger> Logger)
		{
			this.Logger = Logger;
		}

		[Function(nameof(UserHttpTrigger.AddUser))]
		[OpenApiOperation(operationId: "addUser", tags: new[] { "users" }, Summary = "Add a new user to the system", Description = "This adds a new user to the system.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User object that needs to be added to the system")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user details added", Description = "New user details added", Example = typeof(UserHttpTrigger))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			User user = JsonConvert.DeserializeObject<User>(requestBody);

			user.Id += 100;

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(user);

			return response;
		}

		[Function(nameof(UserHttpTrigger.GetUserById))]
		[OpenApiOperation(operationId: "getUserById", tags: new[] { "users" }, Summary = "Find user by ID", Description = "Returns a single user.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of user to return", Description = "ID of user to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(User.DummyUserExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "user/{userId}")] HttpRequestData req, long userId, FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			User user = new User();

			await response.WriteAsJsonAsync(user);

			return response;
		}
	}
}
