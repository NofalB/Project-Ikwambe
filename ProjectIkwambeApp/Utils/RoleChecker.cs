using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using Domain;
using Infrastructure.Services;

namespace ProjectIkwambe.Utils
{
    public class RoleChecker
    {
		ILogger Logger { get; }

		private readonly IUserService _userService;

		public RoleChecker(ILogger<RoleChecker> Logger, IUserService userService)
		{
			this.Logger = Logger;
			_userService = userService;
        }

        internal static async Task<HttpResponseData> ExecuteForUser(Role[] accessLevel, HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate, string userId = null)
        {
            //authenticate for user, need a proper way
            try
            {
				ClaimsPrincipal User = ExecutionContext.GetUser();
				bool allowedRole = CheckUserRole(User, accessLevel);
               
				if (!allowedRole || User.Identity.Name != userId)
                {
                    HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Forbidden);
                    return Response;
                }
				return await executeDefault(Request, ExecutionContext, User, Delegate);

            }
            catch (Exception e)
            {
                HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Response;
            }
        }

		public static async Task<HttpResponseData> executeDefault(HttpRequestData Request, FunctionContext ExecutionContext, ClaimsPrincipal User, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
        {
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
		
		public static bool CheckUserRole(ClaimsPrincipal User, Role[] accessLevel)
        {
			bool allowedRole = false;
			foreach (Role r in accessLevel) //r = user 
			{
				if (User.IsInRole(r.ToString()))
				{
					allowedRole = true;
				}
			}
			return allowedRole;
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
