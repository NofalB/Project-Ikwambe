using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectIkwambe.Models;
using ProjectIkwambe.Service;

namespace ProjectIkwambe.Controllers
{
    public class LoginController
    {
        //ILogger Logger { get; }
        private readonly ITokenService _tokenService;

        public LoginController(ITokenService TokenService/*, ILogger<HelloWorldHttpTrigger> Logger*/)
        {
            this._tokenService = TokenService;
            //this.Logger = Logger;
        }

        [Function(nameof(LoginController.Login))]
        [OpenApiOperation(operationId: "Login", tags: new[] { "Login" }, Summary = "Login for a user",
                            Description = "This method logs in the user, and retrieves a JWT bearer token.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginRequest), Required = true, Description = "The user credentials")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResult), Description = "Login success")]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            LoginRequest login = JsonConvert.DeserializeObject<LoginRequest>(await new StreamReader(req.Body).ReadToEndAsync());

            LoginResult result = await _tokenService.CreateToken(login);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
