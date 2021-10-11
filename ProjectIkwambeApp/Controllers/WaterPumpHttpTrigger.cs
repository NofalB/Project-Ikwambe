using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Worker;
using System.IO;
using Newtonsoft.Json;
using Domain;
using Infrastructure.Services;
using ProjectIkwambe.Attributes;
using ProjectIkwambe.Utils;
using System.Security.Claims;
using Domain.DTO;

namespace ProjectIkwambe.Controllers
{
    public class WaterpumpHttpTrigger
    {
        private readonly IWaterpumpProjectService _waterpumpProjectService;
        ILogger Logger { get; }

        public WaterpumpHttpTrigger(ILogger<WaterpumpHttpTrigger> Logger, IWaterpumpProjectService waterpumpProjectService)
        {
            this.Logger = Logger;
            _waterpumpProjectService = waterpumpProjectService;
        }

        //get all the waterpumps
        [Function(nameof(WaterpumpHttpTrigger.GetWaterpumps))]
        [Auth]
        [OpenApiOperation(operationId: "getWaterpump", tags: new[] { "Waterpumps" }, Summary = "Find all waterpumps", Description = "return all waterpumps", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterpumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterpumpProjectExamples))]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpumps not found", Description = "waterpumps not found")]
        public async Task<HttpResponseData> GetWaterpumps([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterpumps")] HttpRequestData req, FunctionContext executionContext) 
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(await _waterpumpProjectService.GetAllWaterPumpProjects());
           
            return response;

            /*return await RoleChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal Admin) => {

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(_waterpumpProjectService.GetAllWaterPumpProjects());

                return response;
            });*/
        }

        //get water pump information by ID
        [Function(nameof(WaterpumpHttpTrigger.GetWaterpumpById))]
        [OpenApiOperation(operationId: "getWaterpumpById", tags: new[] { "Waterpumps" }, Summary = "Find waterpump by ID", Description = "Returns a single waterpump.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "waterPumpId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of waterpump to return", Description = "ID of waterpump to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterpumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterpumpProjectExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump details not found", Description = "waterpump details not found")]
        public async Task<HttpResponseData> GetWaterpumpById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterpumps/{waterPumpId}")] HttpRequestData req, string waterpumpId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(await _waterpumpProjectService.GetWaterPumpProjectById(waterpumpId));

            return response;
        }

        //add water pumps
        [Function(nameof(WaterpumpHttpTrigger.AddWaterpumps))]
        [OpenApiOperation(operationId: "addWaterPumps", tags: new[] { "Waterpumps" }, Summary = "Add a new waterpump to the database", Description = "This add waterpump information to the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WaterpumpProjectDTO), Required = true, Description = "waterpump object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterpumpProjectDTO), Summary = "New waterpump details added", Description = "New waterpump details added to the database", Example = typeof(DummyWaterpumpProjectExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> AddWaterpumps([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "waterpumps")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            WaterpumpProjectDTO waterpumpDTO = JsonConvert.DeserializeObject<WaterpumpProjectDTO>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _waterpumpProjectService.AddWaterpumpProject(waterpumpDTO));
            return response;
        }

        //edit waterpump by id
        [Function(nameof(WaterpumpHttpTrigger.UpdateWaterpump))]
        [OpenApiOperation(operationId: "updatWaterPump", tags: new[] { "Waterpumps" }, Summary = "Update an existing waterpump information", Description = "This updates an existing waterpump.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WaterpumpProject), Required = true, Description = "waterpump object that needs to be change in the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterpumpProject), Summary = "updated the waterpump details", Description = "waterpump details is updated", Example = typeof(WaterpumpProject))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid waterpump ID supplied", Description = "The waterpump ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump not found", Description = "waterpump not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdateWaterpump([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "waterpumps")] HttpRequestData req, FunctionContext executionContext)
        {
            //take the input
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            WaterpumpProject waterPumpProject = JsonConvert.DeserializeObject<WaterpumpProject>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(await _waterpumpProjectService.UpdateWaterPumpProject(waterPumpProject));

            return response;
        }

        //delete waterpump by id
        [Function(nameof(WaterpumpHttpTrigger.DeleteWaterpump))]
        [OpenApiOperation(operationId: "deleteWaterpump", tags: new[] { "Waterpumps" }, Summary = "Delete waterpump", Description = "Delete an existing waterpump details from the database", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "waterpumpId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The id of the waterpump to be deleted", Description = "Delete the waterpump from the database using the id provided", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Accepted, contentType: "application/json", bodyType: typeof(WaterpumpProject), Summary = "Delete the waterpump details", Description = "waterpump details is removed", Example = typeof(WaterpumpProject))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid waterpump ID supplied", Description = "The waterpump ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump not found", Description = "waterpump not found")]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> DeleteWaterpump([HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "waterpumps/{waterpumpId}")] HttpRequestData req, string waterpumpId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Accepted);

            await _waterpumpProjectService.DeleteWaterpumpProjectAsync(waterpumpId);

            return response;
        }

        
        [Function(nameof(WaterpumpHttpTrigger.GetWaterpumpByQuery))]
        [Auth]
        [OpenApiOperation(operationId: "GetWaterpumpByQuery", tags: new[] { "Waterpumps" }, Summary = "query all waterpumps", Description = "return query waterpumps", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "projecttype", In = ParameterLocation.Query, Required = false, Type = typeof(List<ProjectType>), Summary = "project type value", Description = "Project type values that need to be considered for filter", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "projectName", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The name of the waterpump", Description = "the waterpump from the database using the name provided", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterpumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterpumpProjectExamples))]
        public async Task<HttpResponseData> GetWaterpumpByQuery([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterpumps/query")] HttpRequestData req, ProjectType projectType, FunctionContext executionContext)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            List<WaterpumpProject> wp = new List<WaterpumpProject>();

            //wp.Add(await response.WriteAsJsonAsync(await _waterpumpProjectService.GetWaterPumpByProjectType(projectType)));

            return response;
        }
    }
}
