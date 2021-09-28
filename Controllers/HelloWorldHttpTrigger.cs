using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using ProjectIkwambe.Configurations;
using ProjectIkwambe.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectIkwambe.Controllers
{
    public class HelloWorldHttpTrigger
    {
		ILogger Logger { get; }

		public HelloWorldHttpTrigger(ILogger<HelloWorldHttpTrigger> Logger)
		{
			this.Logger = Logger;
		}

		[Function(nameof(HelloWorldHttpTrigger.HelloWorld))]
		[Auth]
		[OpenApiOperation(operationId: "HelloWorld", tags: new[] { "hello world" })]
		[OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Name", Description = "The name to greet", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
		//[UnauthorizedResponse]
		//[ForbiddenResponse]
		public async Task<HttpResponseData> HelloWorld([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext executionContext)
		{
			return await RoleChecker.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
				Logger.LogInformation("C# HTTP trigger function processed a request.");

				Dictionary<string, StringValues> QueryParams = QueryHelpers.ParseQuery(req.Url.Query);

				string name = QueryParams["name"];

				HttpResponseData Response = req.CreateResponse(HttpStatusCode.OK);
				Response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

				Response.WriteString($"{User.Identity.Name} greets you, {name}");

				return Response;
			});
		}
	}
}
