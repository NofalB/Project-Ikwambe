using Domain;
using Infrastructure.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
		[OpenApiOperation(tags: new[] { "Donations" }, Summary = "Get all donations", Description = "This will retrieve all donations", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "donationId", In = ParameterLocation.Query, Required = false, Type = typeof(int), Summary = "ID of donation to return", Description = "Retrieves a specific donation by ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = false, Type = typeof(int), Summary = "ID of user to return donations", Description = "Retrieves donations with this user ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "projectId", In = ParameterLocation.Query, Required = false, Type = typeof(int), Summary = "ID of project to return donations", Description = "Retrieves donations with this project ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "Successfully fetched donations", Description = "Donations successfully retrieved", Example = typeof(DummyDonationsExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid donation ID", Description = "Invalid donation ID was provided")]
		public async Task<HttpResponseData> GetDonations([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "donations")] HttpRequestData req, FunctionContext executionContext)
		{
			{
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

				var donations = await _donationService.GetAllDonationsAsync();

				await response.WriteAsJsonAsync(donations);

				return response;
			}
		}

		[Function(nameof(DonationHttpTrigger.MakeDonation))]
		[OpenApiOperation(operationId: "donation", tags: new[] { "Donations" }, Summary = "Make a donation", Description = "This will make a donation", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Donation), Required = true, Description = "Donation object for donation details")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "New donation details included", Description = "New donation details included", Example = typeof(DummyDonationExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> MakeDonation([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "donations")] HttpRequestData req, FunctionContext executionContext)
		{

			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Donation donation = JsonConvert.DeserializeObject<Donation>(requestBody);

			await _donationService.AddDonation(donation);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(donation);

			return response;
		}
	}
}
