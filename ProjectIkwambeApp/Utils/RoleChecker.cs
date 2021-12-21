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
            //authenticate for user
            try
            {
				ClaimsPrincipal User = ExecutionContext.GetUser();
				bool allowedRole = CheckUserRole(User, accessLevel);

				HttpResponseData reponse = Request.CreateResponse(HttpStatusCode.Forbidden);

				if (allowedRole == true)
                {
					if(!User.IsInRole("Admin"))
                    {
                        if (User.Identity.Name != userId)
                        {
							return reponse;
						}
                        else
                        {
							return await executeDefault(Request, ExecutionContext, User, Delegate);
						}
					}
                    else
                    {
						return await executeDefault(Request, ExecutionContext, User, Delegate);
					}
				}
				return reponse;

            }
            catch (Exception e)
            {
                HttpResponseData response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				
				var responseData = new
				{
					Status = HttpStatusCode.Unauthorized,
					Message = "User is not authorized to access resource."
				};

				await response.WriteAsJsonAsync(responseData);
				response.StatusCode = HttpStatusCode.Unauthorized;
				return response;
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
				HttpResponseData response = Request.CreateResponse(HttpStatusCode.BadRequest);

				var responseData = new
				{
					Status = HttpStatusCode.BadRequest,
					Message = e.GetBaseException().Message
				};

				await response.WriteAsJsonAsync(responseData);
				response.StatusCode = HttpStatusCode.BadRequest;

				return response;
			}
		}
		
		public static bool CheckUserRole(ClaimsPrincipal User, Role[] accessLevel)
        {
			bool allowedRole = false;
			foreach (Role role in accessLevel) //r = user 
			{
				if (User.IsInRole(role.ToString()))
				{
					allowedRole = true;
				}
			}
			return allowedRole;
		}
	}
}
