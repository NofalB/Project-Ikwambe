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
using static ProjectIkwambe.Models.waterPumpProject;

namespace ProjectIkwambe.Controllers
{
    public class WaterPumpHttpTrigger
    {
        ILogger Logger { get; }

        public WaterPumpHttpTrigger(ILogger<WaterPumpHttpTrigger> Logger)
        {
            this.Logger = Logger;
        }

        //get all the waterpumps
        [Function(nameof(WaterPumpHttpTrigger.GetWaterPumps))]
        [OpenApiOperation(operationId: "getWaterPump", tags: new[] { "waterpump" }, Summary = "Find all waterpumps", Description = "return all waterpumps", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(waterPumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterPumpProjectExamples))]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        //[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        public async Task<HttpResponseData> GetWaterPumps([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterPump")] HttpRequestData req, FunctionContext executionContext) 
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            waterPumpProject waterpump = new waterPumpProject();

            await response.WriteAsJsonAsync(waterpump);

            return response;
        }

        //get water pump information by ID
        [Function(nameof(WaterPumpHttpTrigger.GetWaterPumpById))]
        [OpenApiOperation(operationId: "getWaterPumpById", tags: new[] { "waterpump" }, Summary = "Find waterpump by ID", Description = "Returns a single waterpump.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "waterPumpId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of waterpump to return", Description = "ID of waterpump to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(waterPumpProject), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyWaterPumpProjectExamples))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump details not found", Description = "waterpump details not found")]
        public async Task<HttpResponseData> GetWaterPumpById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "waterPump/{waterPumpId}")] HttpRequestData req, long waterPumpId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            waterPumpProject waterpump = new waterPumpProject();

            await response.WriteAsJsonAsync(waterpump);

            return response;
        }

        //add water pumps
        [Function(nameof(WaterPumpHttpTrigger.AddWaterPumps))]
        [OpenApiOperation(operationId: "addWaterPumps", tags: new[] { "waterpump" }, Summary = "Add a new waterpump to the database", Description = "This add waterpump information to the database.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(waterPumpProject), Required = true, Description = "waterpump object that needs to be added to the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(waterPumpProject), Summary = "New waterpump details added", Description = "New waterpump details added to the database", Example = typeof(DummyWaterPumpProjectExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> AddWaterPumps([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "waterPump")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            waterPumpProject waterpump = JsonConvert.DeserializeObject<waterPumpProject>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(waterpump);

            return response;
        }

        //edit waterpump by id
        [Function(nameof(WaterPumpHttpTrigger.UpdatWaterPump))]
        [OpenApiOperation(operationId: "updatWaterPump", tags: new[] { "waterpump" }, Summary = "Update an existing waterpump information", Description = "This updates an existing waterpump.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(waterPumpProject), Required = true, Description = "waterpump object that needs to be change in the database")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(waterPumpProject), Summary = "updated the waterpump details", Description = "waterpump details is updated", Example = typeof(waterPumpProject))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid waterpump ID supplied", Description = "The waterpump ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump not found", Description = "waterpump not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdatWaterPump([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "waterPump")] HttpRequestData req, FunctionContext executionContext)
        {
            //take the input
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            waterPumpProject waterPumpProject = JsonConvert.DeserializeObject<waterPumpProject>(requestBody);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(waterPumpProject);

            return response;
        }

        //delete waterpump by id
        [Function(nameof(WaterPumpHttpTrigger.DeleteWaterPump))]
        [OpenApiOperation(operationId: "deleteWaterPump", tags: new[] { "waterpump" }, Summary = "Delete waterpump", Description = "Delete an existing waterpump details from the database", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(waterPumpProject), Required = true, Description = "waterpump object that will be deleted from the database")]
        [OpenApiParameter(name: "waterPumpId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "The id of the waterpump to be deleted", Description = "Delete the waterpump from the database using the id provided", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(waterPumpProject), Summary = "Delete the waterpump details", Description = "waterpump details is removed", Example = typeof(waterPumpProject))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid waterpump ID supplied", Description = "The waterpump ID does not exist or invalid ID ")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "waterpump not found", Description = "waterpump not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> DeleteWaterPump([HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = "waterPump")] HttpRequestData req, long waterPumpId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);


            //return response;
            return null;
        }




    }
}
