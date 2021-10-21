using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Infrastructure.Services;
using Domain.DTO;
using Microsoft.Extensions.Logging;
using ProjectIkwambe.Attributes;

namespace ProjectIkwambe.Controllers
{
    public class LoginHttpTrigger
    {
        ILogger Logger { get; }
        private readonly ITokenService _tokenService;

        public LoginHttpTrigger(ITokenService TokenService, ILogger<LoginHttpTrigger> Logger)
        {
            this._tokenService = TokenService;
            this.Logger = Logger;
        }

        [Function(nameof(LoginHttpTrigger.Login))]
        [OpenApiOperation(operationId: "Login", tags: new[] { "Login" }, Summary = "Login for a user", Description = "This method logs in the user, and retrieves a JWT bearer token.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginRequest), Required = true, Description = "The user credentials")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResult), Description = "Login success")]
        [UnauthorizedResponse]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequestData req, FunctionContext executionContext)
        {
            LoginRequest login = JsonConvert.DeserializeObject<LoginRequest>(await new StreamReader(req.Body).ReadToEndAsync());

            LoginResult result = await _tokenService.CreateToken(login);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
