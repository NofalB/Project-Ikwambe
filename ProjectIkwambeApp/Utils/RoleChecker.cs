using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using Domain;

namespace ProjectIkwambe.Utils
{
    class RoleChecker
    {
		ILogger Logger { get; }

		public RoleChecker(ILogger<RoleChecker> Logger)
		{
			this.Logger = Logger;
        }

        internal static async Task<HttpResponseData> ExecuteForUser(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate, Role accessLevel, string userId = null)
        {
            //authenticate for user, need a proper way
            try
            {
				ClaimsPrincipal User = ExecutionContext.GetUser();
				if (!User.IsInRole(accessLevel.ToString()) || User.Identity.Name != userId)
                {
                    HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Forbidden);
                    return Response;
                }
                try
                {
                    return await Delegate(User).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    HttpResponseData Response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    return Response;
                }
            }
            catch (Exception e)
            {
                HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Response;
            }
        }

		internal static async Task<HttpResponseData> ExecuteForAdmin1(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			HttpResponseData Response;
			try
			{
				ClaimsPrincipal Admin = ExecutionContext.GetUser();

				if (!Admin.IsInRole("Admin"))
				{
					Response = Request.CreateResponse(HttpStatusCode.Forbidden);

					return Response;
				}
				try
				{
					return await Delegate(Admin).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					Response = Request.CreateResponse(HttpStatusCode.BadRequest);
					return Response;
				}
			}
			catch (Exception e)
			{
				Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}
	}
}
