using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;

namespace ProjectIkwambe.Utils
{
    class RoleChecker
    {
		ILogger Logger { get; }

		public RoleChecker(ILogger<RoleChecker> Logger)
		{
			this.Logger = Logger;
		}

		// Todo: Move this to a baseclass!!
		/*protected async Task<HttpResponseData> ExecuteForUser(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();
				if (!User.IsInRole("User"))
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
				Logger.LogError(e.Message);
				HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}*/

		internal static async Task<HttpResponseData> ExecuteForUser(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();

				if (!User.IsInRole("User"))
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

		internal static async Task<HttpResponseData> ExecuteForAdmin(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetAdmin();

				if (!User.IsInRole("Admin"))
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
	}
}
