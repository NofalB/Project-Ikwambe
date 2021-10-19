﻿using Domain;
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

		[Function(nameof(DonationHttpTrigger.GetDonations))]
		[Auth]
		[OpenApiOperation(tags: new[] { "Donations" }, Summary = "Get all donations", Description = "This will retrieve all donations", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of user to return donations", Description = "Retrieves donations with this user ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "projectId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "ID of project to return donations", Description = "Retrieves donations with this project ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "donationDate", In = ParameterLocation.Query, Required = false, Type = typeof(DateTime), Summary = " date", Description = "date", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "Successfully fetched donations", Description = "Donations successfully retrieved", Example = typeof(DummyDonationsExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid donation ID", Description = "Invalid donation ID was provided")]
		public async Task<HttpResponseData> GetDonations([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "donations")] HttpRequestData req, FunctionContext executionContext)
		{
            //string userId = HttpUtility.ParseQueryString(req.Url.Query).Get("userId");
            string projectId = HttpUtility.ParseQueryString(req.Url.Query).Get("projectId");
            string date = HttpUtility.ParseQueryString(req.Url.Query).Get("date");
			Role[] roles = { Role.Admin };

			/*return await RoleChecker.ExecuteForUser(roles, req, executionContext, async (ClaimsPrincipal User) =>
			{*/
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			
				await response.WriteAsJsonAsync(_donationService.GetDonationByQueryOrGetAll(projectId, date));

				return response;
			//});
		}

		//get byId
		[Function(nameof(DonationHttpTrigger.GetDonationsById))]
		[Auth]
		[OpenApiOperation(operationId: "getDonationsById", tags: new[] {"Donations"}, Summary = "Find Donation by Id", Description = "return one donation object", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "donationId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of donation to return", Description = "ID of donation to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of donation to return", Description = "ID of donation to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyDonationsExamples))]
		public async Task<HttpResponseData> GetDonationsById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "donations/{donationId}")] HttpRequestData req, string donationId, FunctionContext executionContext)
		{
			string userId = HttpUtility.ParseQueryString(req.Url.Query).Get("userId");

			Role[] roles = { Role.User };

			/*return await RoleChecker.ExecuteForUser(roles, req,  executionContext, async (ClaimsPrincipal User)  =>
			{*/
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

				await response.WriteAsJsonAsync(await _donationService.GetDonationByIdAsync(donationId, userId));

				return response;

			//},userId);
		}

		[Function(nameof(DonationHttpTrigger.MakeDonation))]
		[OpenApiOperation(operationId: "donation", tags: new[] { "Donations" }, Summary = "Make a donation", Description = "This will make a donation", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DonationDTO), Required = true, Description = "Donation object for donation details")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DonationDTO), Summary = "New donation details included", Description = "New donation details included", Example = typeof(DummyDonationDTOExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> MakeDonation([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "donations")] HttpRequestData req, FunctionContext executionContext)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			
			DonationDTO donationDTO = JsonConvert.DeserializeObject<DonationDTO>(requestBody);
			// Generate output
			HttpResponseData response = req.CreateResponse();

			await response.WriteAsJsonAsync(await _donationService.AddDonation(donationDTO));
			response.StatusCode = HttpStatusCode.Created;

			return response;
		}


		[Function(nameof(DonationHttpTrigger.GetDonationByUser))]
		[Auth]
		[OpenApiOperation(operationId: "donation", tags: new[] { "Donations" }, Summary = "Get donation for a specific user", Description = "Collect all Donation for a specific user", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "specificUserId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The userId to get their list of donations made", Description = "getting all the donation for a specific user", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Donation), Required = true, Description = "Return a list of donation based on user Id")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "List of donations", Description = "List of Donation for the specific user", Example = typeof(DummyDonationExample))]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> GetDonationByUser([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "donations/user/{specificUserId}")] HttpRequestData req, string userId, FunctionContext executionContext)
		{
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(_donationService.GetDonationByUserId(userId));

			return response;
		}

	}
}
