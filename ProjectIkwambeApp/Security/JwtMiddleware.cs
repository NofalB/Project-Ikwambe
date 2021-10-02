using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Security
{
    public class JwtMiddleware : IFunctionsWorkerMiddleware
    {
        ITokenService TokenService { get; }
        ILogger Logger { get; }

        public JwtMiddleware(ITokenService TokenService, ILogger<JwtMiddleware> Logger)
        {
            this.TokenService = TokenService;
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext Context, FunctionExecutionDelegate Next)
        {
            string HeadersString = (string)Context.BindingContext.BindingData["Headers"];

            Dictionary<string, string> Headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(HeadersString);

            if (Headers.TryGetValue("Authorization", out string AuthorizationHeader))
            {
                try
                {
                    AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(AuthorizationHeader);

                    ClaimsPrincipal User = await TokenService.GetByValue(BearerHeader.Parameter);

                   // ClaimsPrincipal Admin = await TokenService.GetByValue(BearerHeader.Parameter);

                    Context.Items["User"] = User;

                    //Context.Items["Admin"] = Admin;
                }
                catch (Exception e)
                {
                    Logger.LogError(e.Message);
                }
            }

            await Next(Context);
        }
    }
}
