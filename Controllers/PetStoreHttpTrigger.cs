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

namespace Petstore.Controllers {
	public class PetStoreHttpTrigger {
		ILogger Logger { get; }

		public PetStoreHttpTrigger(ILogger<PetStoreHttpTrigger> Logger) {
			this.Logger = Logger;
		}

		[Function(nameof(PetStoreHttpTrigger.AddPet))]
		[OpenApiOperation(operationId: "addPet", tags: new[] { "pet" }, Summary = "Add a new pet to the store", Description = "This add a new pet to the store.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Pet), Required = true, Description = "Pet object that needs to be added to the store")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "New pet details added", Description = "New pet details added", Example = typeof(DummyPetExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddPet([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "pet")] HttpRequestData req, FunctionContext executionContext) {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Pet pet = JsonConvert.DeserializeObject<Pet>(requestBody);

			pet.Id += 100;

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(pet);

			return response;
		}

		[Function(nameof(PetStoreHttpTrigger.UpdatePet))]
		[OpenApiOperation(operationId: "updatePet", tags: new[] { "pet" }, Summary = "Update an existing pet", Description = "This updates an existing pet.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Pet), Required = true, Description = "Pet object that needs to be updated to the store")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "Pet details updated", Description = "Pet details updated", Example = typeof(DummyPetExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> UpdatePet([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "pet")] HttpRequestData req, FunctionContext executionContext) {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Pet pet = JsonConvert.DeserializeObject<Pet>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(pet);

			return response;
		}

		[Function(nameof(PetStoreHttpTrigger.FindByStatus))]
		[OpenApiOperation(operationId: "findPetsByStatus", tags: new[] { "pet" }, Summary = "Finds Pets by status", Description = "Multiple status values can be provided with comma separated strings.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "status", In = ParameterLocation.Query, Required = true, Type = typeof(List<PetStatus>), Summary = "Pet status value", Description = "Status values that need to be considered for filter", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pet>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyPetExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid status value", Description = "Invalid status value")]
		public async Task<HttpResponseData> FindByStatus([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "pet/findByStatus")] HttpRequestData req, FunctionContext executionContext) {
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			List<Pet> pets = new List<Pet>() { };

			await response.WriteAsJsonAsync(pets);

			return response;
		}

		[Function(nameof(PetStoreHttpTrigger.FindByTags))]
		[OpenApiOperation(operationId: "findPetsByTags", tags: new[] { "pet" }, Summary = "Finds Pets by tags", Description = "Muliple tags can be provided with comma separated strings.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "tags", In = ParameterLocation.Query, Required = true, Type = typeof(List<string>), Summary = "Tags to filter by", Description = "Tags to filter by", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pet>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyPetExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid tag value", Description = "Invalid tag value")]
		public async Task<HttpResponseData> FindByTags([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "pet/findByTags")] HttpRequestData req, FunctionContext executionContext) {
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			List<Pet> pets = new List<Pet>() { };

			await response.WriteAsJsonAsync(pets);

			return response;
		}

		[Function(nameof(PetStoreHttpTrigger.GetPetById))]
		[OpenApiOperation(operationId: "getPetById", tags: new[] { "pet" }, Summary = "Find pet by ID", Description = "Returns a single pet.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "petId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of pet to return", Description = "ID of pet to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyPetExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
		public async Task<HttpResponseData> GetPetById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "pet/{petId}")] HttpRequestData req, long petId, FunctionContext executionContext) {
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			Pet pet = new Pet();

			await response.WriteAsJsonAsync(pet);

			return response;
		}
	}
}
