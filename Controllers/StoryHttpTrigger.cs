using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectIkwambe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ProjectIkwambe.Models.Story;

namespace ProjectIkwambe.Controllers
{
    public class StoryHttpTrigger
    {
        ILogger Logger { get; }

        public StoryHttpTrigger(ILogger<StoryHttpTrigger> Logger)
        {
            this.Logger = Logger;
        }


        //get all story
        [Function(nameof(StoryHttpTrigger.GetStories))]
        [OpenApiOperation(operationId: "getStories", tags: new[] { "Stories" }, Summary = "Get all stories", Description = "return all stories", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Story), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyStoryExamples))]
        public async Task<HttpResponseData> GetStories([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "stories")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            Story story = new Story();

            await response.WriteAsJsonAsync(story);

            return response;
        }

        //get story by id
        [Function(nameof(StoryHttpTrigger.GetStoryById))]
        [OpenApiOperation(operationId: "getStoryById", tags: new[] { "Stories" }, Summary = "Find story by ID", Description = "Returns a single story object.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "storyId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of the story", Description = "ID of story object to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Story), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyStoryExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "story details not found", Description = "story details not found")]
        public async Task<HttpResponseData> GetStoryById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "stories/{storyId}")] HttpRequestData req, long storyId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            Story story = new Story();

            await response.WriteAsJsonAsync(story);

            return response;
        }

        //post story
        [Function(nameof(StoryHttpTrigger.AddStory))]
        [OpenApiOperation(operationId: "addStory", tags: new[] { "Stories" }, Summary = "Add a new story to the database", Description = "This method add story information to the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Story), Required = true, Description = "story object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Story), Summary = "New story details added", Description = "New story details added to the database", Example = typeof(DummyStoryExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> AddStory([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "stories")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Story story = JsonConvert.DeserializeObject<Story>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(story);

            return response;
        }

        //edit story
        [Function(nameof(StoryHttpTrigger.UpdateStory))]
        [OpenApiOperation(operationId: "updateStory", tags: new[] { "Stories" }, Summary = "Update an existing story", Description = "This updates an existing story.", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Story), Required = true, Description = "story object that needs to be changed in the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Story), Summary = "Story details updated", Description = "Story details updated", Example = typeof(DummyStoryExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Story not found", Description = "Story not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdateStory([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "stories")] HttpRequestData req, FunctionContext executionContext)
        {
            // Parse input
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Story story = JsonConvert.DeserializeObject<Story>(requestBody);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(story);

            return response;
        }

        //delete story
        [Function(nameof(StoryHttpTrigger.DeleteStory))]
        [OpenApiOperation(operationId: "deleteStory", tags: new[] { "Stories" }, Summary = "Delete the story", Description = "Delete an existing story details from the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "storyId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "The id of the story to be deleted", Description = "Delete the story from the database using the Id provided", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Story), Summary = "Delete the story details", Description = "story details is removed", Example = typeof(DummyStoryExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid story ID supplied", Description = "The story ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Story not found", Description = "Story not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> DeleteStory([HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "stories/{storyId}")] HttpRequestData req, long storyId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }

    }
}
