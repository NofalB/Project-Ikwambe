using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ProjectIkwambe.Controllers
{
	class UserHttpTrigger
	{
		ILogger Logger { get; }

		public UserHttpTrigger(ILogger<UserHttpTrigger> Logger)
		{
			this.Logger = Logger;
		}

		[Function(nameof(UserHttpTrigger.GetUsers))]
		[OpenApiOperation(operationId: "getUsers", tags: new[] { "Users" }, Summary = "Get all users", Description = "Returns a list of users.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Check connection", Description = "Check connection")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "users")] HttpRequestData req, FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			List<User> users = new List<User>();
			users.Add(new User("100", "Kratos", "Jumbo", "bruh@gmail.com", "100",true));
			users.Add(new User("101", "Bam", "Test", "bruh@gmail.com", "602",false));
			users.Add(new User("102", "Jumbo", "Kratos", "bruh@gmail.com", "750",true));


			await response.WriteAsJsonAsync(users);

			return response;
		}

		[Function(nameof(UserHttpTrigger.AddUser))]
		[OpenApiOperation(operationId: "addUser", tags: new[] { "Users" }, Summary = "Add a new user to the system", Description = "This adds a new user to the system.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User object that needs to be added to the system")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user details added", Description = "New user details added", Example = typeof(UserHttpTrigger))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "users")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			User user = JsonConvert.DeserializeObject<User>(requestBody);

			user.UserId += 1;

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(user);

			return response;
		}

		[Function(nameof(UserHttpTrigger.GetUserById))]
		[OpenApiOperation(operationId: "getUserById", tags: new[] { "Users" }, Summary = "Find user by ID", Description = "Returns a single user.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of user to return", Description = "ID of user to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid user ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "users/{userId}")] HttpRequestData req, long userId, FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			User user = new User("100", "Kratos", "Jumbo", "bruh@gmail.com", "HSJSH6767",true);

			await response.WriteAsJsonAsync(user);

			return response;
		}

		
		[Function(nameof(UserHttpTrigger.DeleteUser))]
		[OpenApiOperation(operationId: "deleteUser", tags: new[] { "Users" }, Summary = "Delete user by ID", Description = "Delete an existing user by ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of user to return", Description = "ID of user to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successfull operation", Description = "the user has been deleted successfully", Example = typeof(User))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid user ID supplied", Description = "The user ID is invalid ")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "user not found", Description = "user not found by the inserted ID,please check again")]
		public Task<HttpResponseData> DeleteUser([HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "users/{userId}")] HttpRequestData req, long userId, FunctionContext executionContext)
		{
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			return Task.FromResult(response);
		}

		[Function(nameof(UserHttpTrigger.UpdateUser))]
		[OpenApiOperation(operationId: "updateUser", tags: new[] { "Users" }, Summary = "update an existing user in the system", Description = "Updates an existing user by user id", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User object that needs to be updated in the system")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "User details updated", Description = "The user has been sucessfully updated", Example = typeof(DummyUserExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "users/{userId}")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			User user = JsonConvert.DeserializeObject<User>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(user);

			return response;
		}
	}
}
