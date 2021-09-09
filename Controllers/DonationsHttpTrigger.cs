using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Controllers
{
	public class DonationsHttpTrigger
	{
		ILogger Logger { get; }

		public DonationsHttpTrigger(ILogger<DonationsHttpTrigger> Logger)
		{
			this.Logger = Logger;
		}

		[Function(nameof(DonationsHttpTrigger.GetDonations))]
		[OpenApiOperation(tags: new[] { "donation" }, Summary = "Get all donations", Description = "This will retrieve all donations", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "Successful donation operation", Description = "Donations Successfully Retrieved", Example = typeof(DummyDonationsExamples))]
		public async Task<HttpResponseData> GetDonations([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "donations")] HttpRequestData req, FunctionContext executionContext)
		{
			{
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

				Donation donation = new Donation();

				await response.WriteAsJsonAsync(donation);

				return response;
			}
		}

		[Function(nameof(DonationsHttpTrigger.MakeDonation))]
		[OpenApiOperation(operationId: "donation", tags: new[] { "donation" }, Summary = "Make a donation", Description = "This will make a donation", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Donation), Required = true, Description = "Donation object for donation details")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "New donation details included", Description = "New donation details included", Example = typeof(DummyDonationExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> MakeDonation([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "donation")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Donation donation = JsonConvert.DeserializeObject<Donation>(requestBody);

			donation.DonationId += 100;

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(donation);

			return response;
		}

		[Function(nameof(DonationsHttpTrigger.UpdateDonation))]
		[OpenApiOperation(operationId: "updateDonation", tags: new[] { "donation" }, Summary = "Update an existing donation", Description = "This updates an existing donation.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Donation), Required = true, Description = "Donation object that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Donation), Summary = "Donation details updated", Description = "Donation details updated", Example = typeof(DummyDonationExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Donation not found", Description = "Donation not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> UpdateDonation([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "donation")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Donation donation = JsonConvert.DeserializeObject<Donation>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(donation);

			return response;
		}
	}
}
