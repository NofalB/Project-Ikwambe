using Domain;
using Domain.DTO;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectIkwambe.Attributes;
using ProjectIkwambe.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProjectIkwambe.Controllers
{
	public class DonationHttpTrigger
	{
		private ILogger Logger { get; }
		private readonly IDonationService _donationService;

		public DonationHttpTrigger(ILogger<DonationHttpTrigger> Logger, IDonationService donationService)
		{
			this.Logger = Logger;
			_donationService = donationService;
		}

		[Function(nameof(DonationHttpTrigger.GetAllDonations))]
		[Auth]
		[OpenApiOperation(operationId: "GetAllDonations", tags: new[] { "Donations" }, Summary = "Get all donations", Description = "This will retrieve all donations", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "projectId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "ID of project to return donations", Description = "Retrieves donations with this project ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "donationDate", In = ParameterLocation.Query, Required = false, Type = typeof(DateTime), Summary = "The date of donation", Description = "This will retrieve the donations by a specific date", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "Successfully fetched donations", Description = "Donations successfully retrieved", Example = typeof(DummyDonationsExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid donation ID", Description = "Invalid donation ID was provided")]
		public async Task<HttpResponseData> GetAllDonations([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "donations")] HttpRequestData req, FunctionContext executionContext)
		{
            string projectId = HttpUtility.ParseQueryString(req.Url.Query).Get("projectId");
            string donationDate = HttpUtility.ParseQueryString(req.Url.Query).Get("donationDate");
			Role[] roles = { Role.Admin };

			return await RoleChecker.ExecuteForUser(roles, req, executionContext, async (ClaimsPrincipal User) =>
			{
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			
				await response.WriteAsJsonAsync(await _donationService.GetDonationByQueryOrGetAllAsync(projectId, donationDate));

				return response;
			});
		}

		[Function(nameof(DonationHttpTrigger.GetDonationsById))]
		[Auth]
		[OpenApiOperation(operationId: "GetDonationsById", tags: new[] {"Donations"}, Summary = "Find Donation by Id", Description = "return one donation object", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "donationId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of donation to return", Description = "ID of donation to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyDonationsExamples))]
		public async Task<HttpResponseData> GetDonationsById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "donations/{donationId}")] HttpRequestData req, string donationId, FunctionContext executionContext)
		{

			Role[] roles = { Role.User };

			return await RoleChecker.ExecuteForUser(roles, req,  executionContext, async (ClaimsPrincipal User)  =>
			{
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

				await response.WriteAsJsonAsync(await _donationService.GetDonationByIdAsync(donationId));

				return response;

			});
		}

		[Function(nameof(DonationHttpTrigger.GetDonationsByUser))]
		[Auth]
		[OpenApiOperation(operationId: "GetDonationsByUser", tags: new[] { "Donations" }, Summary = "Get donation for a specific user", Description = "Collect all Donation for a specific user", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The userId to get their list of donations made", Description = "getting all the donation for a specific user", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "List of donations", Description = "List of Donation for the specific user", Example = typeof(DummyDonationExample))]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> GetDonationsByUser([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "donations/user/{userId}")] HttpRequestData req, string userId, FunctionContext executionContext)
		{
			Role[] roles = { Role.User };

			return await RoleChecker.ExecuteForUser(roles, req, executionContext, async (ClaimsPrincipal User) =>
			{
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

				await response.WriteAsJsonAsync(await _donationService.GetDonationByUserIdAsync(userId));

				return response;
			}, userId);
		}

	}
}
