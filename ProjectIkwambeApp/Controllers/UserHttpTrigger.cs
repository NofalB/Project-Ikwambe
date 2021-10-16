using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Domain;
using Domain.DTO;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectIkwambe.Utils;

namespace ProjectIkwambe.Controllers
{
	class UserHttpTrigger
	{
		ILogger Logger { get; }
		private readonly IUserService _userService;

		public UserHttpTrigger(ILogger<UserHttpTrigger> Logger, IUserService userService)
		{
			this.Logger = Logger;
			_userService = userService;
		}

		[Function(nameof(UserHttpTrigger.GetUsers))]
		[OpenApiOperation(operationId: "getUsers", tags: new[] { "Users" }, Summary = "Get all users", Description = "Returns a list of users.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "firstName", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "The firstname of the user", Description = "The firstname data from the database using the firstname provided", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "lasttName", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "The lastname of the user", Description = "The lastname data from the database using the lastname provided", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "subscription", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "The subcription of the user", Description = "return a list of user with their subscription", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Check connection", Description = "Check connection")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users")] HttpRequestData req, FunctionContext executionContext)
		{
			string firstName = HttpUtility.ParseQueryString(req.Url.Query).Get("firstName");
			string lastName = HttpUtility.ParseQueryString(req.Url.Query).Get("lasttName");
			string subcription = HttpUtility.ParseQueryString(req.Url.Query).Get("subscription");

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			//await response.WriteAsJsonAsync(await _userService.GetAllUsers());
			await response.WriteAsJsonAsync(_userService.GetUserByQueryOrGetAll(firstName, lastName, subcription));

			return response;
		}

		[Function(nameof(UserHttpTrigger.GetUserById))]
		[OpenApiOperation(operationId: "getUserById", tags: new[] { "Users" }, Summary = "Find user by ID", Description = "Returns a single user.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of user to return", Description = "ID of user to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid user ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users/{userId}")] HttpRequestData req, string userId, FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			await response.WriteAsJsonAsync(await _userService.GetUserById(userId));

			return response;
		}

		[Function(nameof(UserHttpTrigger.AddUser))]
		[OpenApiOperation(operationId: "addUser", tags: new[] { "Users" }, Summary = "Add a new user to the system", Description = "This adds a new user to the system.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDTO), Required = true, Description = "User object that needs to be added to the system")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user details added", Description = "New user details added", Example = typeof(UserHttpTrigger))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "users")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
			await response.WriteAsJsonAsync(await _userService.AddUser(userDTO));
			return response;
		}

		[Function(nameof(UserHttpTrigger.UpdateUser))]
		[OpenApiOperation(operationId: "updateUser", tags: new[] { "Users" }, Summary = "update an existing user in the system", Description = "Updates an existing user by user id", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User object that needs to be updated in the system")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "User details updated", Description = "The user has been sucessfully updated", Example = typeof(DummyUserExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "users/{userId}")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			User user = JsonConvert.DeserializeObject<User>(requestBody);
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			await response.WriteAsJsonAsync(await _userService.UpdateUser(user));
			return response;
		}

		
		[Function(nameof(UserHttpTrigger.DeleteUser))]
		[OpenApiOperation(operationId: "deleteUser", tags: new[] { "Users" }, Summary = "Delete user by ID", Description = "Delete an existing user by ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of user to return", Description = "ID of user to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.Accepted, contentType: "application/json", bodyType: typeof(User), Summary = "successfull operation", Description = "the user has been deleted successfully", Example = typeof(User))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid user ID supplied", Description = "The user ID is invalid ")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "user not found", Description = "user not found by the inserted ID,please check again")]
		public async Task<HttpResponseData> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "users/{userId}")] HttpRequestData req, string userId, FunctionContext executionContext)
		{
			return await RoleChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
				HttpResponseData response = req.CreateResponse(HttpStatusCode.Accepted);
				await _userService.DeleteUserAsync(userId);
				return response;
			}, Role.Admin);
		}
	}
}
