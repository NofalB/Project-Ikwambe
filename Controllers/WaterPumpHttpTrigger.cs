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
using ProjectIkwambe.Models;
using static ProjectIkwambe.Models.WaterPumpProject;
using System.Security.Claims;
using ProjectIkwambe.Configurations;
using ProjectIkwambe.Utils;

namespace ProjectIkwambe.Controllers
{
    public class WaterpumpHttpTrigger
    {
        ILogger Logger { get; }

        public WaterpumpHttpTrigger(ILogger<WaterpumpHttpTrigger> Logger)
        {
            this.Logger = Logger;
        }

        //get all the waterpumps
        [Function(nameof(WaterpumpHttpTrigger.GetWaterpumps))]
        [Auth]
        [OpenApiOperation(operationId: "getWaterpump", tags: new[] { "Waterpumps" }, Summary = "Find all waterpumps", Description = "return all waterpumps", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterPumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterPumpProjectExamples))]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        public async Task<HttpResponseData> GetWaterpumps([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterpumps")] HttpRequestData req, FunctionContext executionContext) 
        {
            return await RoleChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                WaterPumpProject waterpump = new WaterPumpProject();

                await response.WriteAsJsonAsync(waterpump);

                return response;
            });

            /*HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            WaterPumpProject waterpump = new WaterPumpProject();

            await response.WriteAsJsonAsync(waterpump);

            return response;*/
        }

        //get water pump information by ID
        [Function(nameof(WaterpumpHttpTrigger.GetWaterpumpById))]
        [OpenApiOperation(operationId: "getWaterpumpById", tags: new[] { "Waterpumps" }, Summary = "Find waterpump by ID", Description = "Returns a single waterpump.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "waterPumpId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of waterpump to return", Description = "ID of waterpump to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterPumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterPumpProjectExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump details not found", Description = "waterpump details not found")]
        public async Task<HttpResponseData> GetWaterpumpById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterpumps/{waterPumpId}")] HttpRequestData req, long waterPumpId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            WaterPumpProject waterpump = new WaterPumpProject();

            await response.WriteAsJsonAsync(waterpump);

            return response;
        }

        //add water pumps
        [Function(nameof(WaterpumpHttpTrigger.AddWaterpumps))]
        [OpenApiOperation(operationId: "addWaterpump", tags: new[] { "Waterpumps" }, Summary = "Add a new waterpump to the database", Description = "This add waterpump information to the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WaterPumpProject), Required = true, Description = "waterpump object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterPumpProject), Summary = "New waterpump details added", Description = "New waterpump details added to the database", Example = typeof(DummyWaterPumpProjectExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> AddWaterpumps([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "waterpumps")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            WaterPumpProject waterpump = JsonConvert.DeserializeObject<WaterPumpProject>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(waterpump);

            return response;
        }

        //edit waterpump by id
        [Function(nameof(WaterpumpHttpTrigger.UpdatWaterpump))]
        [OpenApiOperation(operationId: "updatWaterpump", tags: new[] { "Waterpumps" }, Summary = "Update an existing waterpump information", Description = "This updates an existing waterpump.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WaterPumpProject), Required = true, Description = "waterpump object that needs to be change in the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterPumpProject), Summary = "updated the waterpump details", Description = "waterpump details is updated", Example = typeof(WaterPumpProject))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid waterpump ID supplied", Description = "The waterpump ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump not found", Description = "waterpump not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdatWaterpump([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "waterpumps")] HttpRequestData req, FunctionContext executionContext)
        {
            //take the input
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            WaterPumpProject waterPumpProject = JsonConvert.DeserializeObject<WaterPumpProject>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(waterPumpProject);

            return response;
        }

        //delete waterpump by id
        [Function(nameof(WaterpumpHttpTrigger.DeleteWaterpump))]
        [OpenApiOperation(operationId: "deleteWaterpump", tags: new[] { "Waterpumps" }, Summary = "Delete waterpump", Description = "Delete an existing waterpump details from the database", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(waterpumpProject), Required = true, Description = "waterpump object that will be deleted from the database")]
        [OpenApiParameter(name: "waterPumpId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "The id of the waterpump to be deleted", Description = "Delete the waterpump from the database using the id provided", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WaterPumpProject), Summary = "Delete the waterpump details", Description = "waterpump details is removed", Example = typeof(WaterPumpProject))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid waterpump ID supplied", Description = "The waterpump ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump not found", Description = "waterpump not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> DeleteWaterpump([HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "waterpumps")] HttpRequestData req, long waterPumpId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);


            //return response;
            return null;
        }




    }
}
