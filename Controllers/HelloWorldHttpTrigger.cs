using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;

namespace ProjectIkwambe.Controllers {
	public class HelloWorldHttpTrigger {
		ILogger Logger { get; }

		public HelloWorldHttpTrigger(ILogger<HelloWorldHttpTrigger> Logger) {
			this.Logger = Logger;
		}

		[Function(nameof(HelloWorldHttpTrigger.HelloWorld))]
		[OpenApiOperation(operationId: "HelloWorld", tags: new[] { "hello world" })]
		[OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Name", Description = "The name to greet", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
		public HttpResponseData HelloWorld([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext executionContext) {
			Logger.LogInformation("C# HTTP trigger function processed a request.");

			Dictionary<string, StringValues> queryParams = QueryHelpers.ParseQuery(req.Url.Query);

			string name = queryParams["name"];

			Logger.LogInformation($"Greeting {name}");

			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

			response.WriteString($"Welcome to Azure Functions, {name}!");

			return response;
		}
	}
}
