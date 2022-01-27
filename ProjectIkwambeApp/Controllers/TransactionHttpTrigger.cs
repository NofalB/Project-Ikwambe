using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Domain;
using Domain.DTO;
using Infrastructure.Services;
using Infrastructure.Services.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProjectIkwambe.Attributes;
using ProjectIkwambe.Utils;

namespace ProjectIkwambe.Controllers
{
    public class TransactionHttpTrigger
    {
		ILogger Logger { get; }
        private ITransactionService _transactionService;
        private IPaypalClientService _paypalClientService;
        public TransactionHttpTrigger(ILogger<TransactionHttpTrigger> Logger, ITransactionService transactionService, IPaypalClientService paypalClientService)
		{
			this.Logger = Logger;
            _transactionService = transactionService;
            _paypalClientService = paypalClientService;

        }

        [Auth]
        [Function(nameof(TransactionHttpTrigger.GetTransactions))]
        [OpenApiOperation(tags: new[] { "DBTransactions" }, Summary = "Get all transactions from the db", Description = "This will retrieve all transactions", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        public async Task<HttpResponseData> GetTransactions([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/db")] HttpRequestData req, FunctionContext executionContext)
        {
            Role[] roles = { Role.Admin };
            return await RoleChecker.ExecuteForUser(roles, req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(await _transactionService.GetAllTransactions());

                return response;
            });
        }

        [Auth]
        [Function(nameof(TransactionHttpTrigger.GetTransactionsById))]
        [OpenApiOperation(tags: new[] { "DBTransactions" }, Summary = "Get a specific transactions from db", Description = "This will retrieve a specific transaction from db", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid transaction ID", Description = "Invalid transaction ID was provided")]
        public async Task<HttpResponseData> GetTransactionsById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/db/{transactionId}")] HttpRequestData req, string transactionId,FunctionContext executionContext)
        {
            Role[] roles = { Role.Admin };
            return await RoleChecker.ExecuteForUser(roles, req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(await _transactionService.GetTransactionById(transactionId));
                return response;
            });
        }

        [Function(nameof(TransactionHttpTrigger.GetTransactionsPayPal))]
        [OpenApiOperation(tags: new[] { "PaypalTransactions" }, Summary = "Get a transaction directly from paypal microservice", Description = "This will retrieve a transaction directly from paypal microservice", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        public async Task<HttpResponseData> GetTransactionsPayPal([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/paypal/{transactionId}")] HttpRequestData req, string transactionId, FunctionContext executionContext)
        {
            var transaction = await _paypalClientService.GetTransaction(transactionId);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transaction);
            return response;
        }

        [Function(nameof(TransactionHttpTrigger.CreateCheckoutUrl))]
		[OpenApiOperation(operationId: "transactionURL", tags: new[] { "PaypalTransactions" }, Summary = "Create a transaction URL", Description = "This will create and return a transaction link", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CheckoutUrl), Summary = "New transaction details", Description = "New transaction details")]
		[OpenApiParameter(name: "currency", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The currency required for the transaction creation", Description = "Creates the transaction with specified currency type", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "value", In = ParameterLocation.Query, Required = true, Type = typeof(int), Summary = "The value/amount required for the transaction creation", Description = "Creates the transaction with specified value amount", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> CreateCheckoutUrl([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/paypal/checkout")] HttpRequestData req, FunctionContext executionContext)
		{
            string currencyCode = HttpUtility.ParseQueryString(req.Url.Query).Get("currency");
            string value = HttpUtility.ParseQueryString(req.Url.Query).Get("value");

            var checkoutUrl = await _paypalClientService.GetCheckoutUrl(currencyCode, value);
            
            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(checkoutUrl);
            return response;
         
        }

        [Function(nameof(TransactionHttpTrigger.CompleteTransaction1))]
        [OpenApiOperation(operationId: "completeTransaction1", tags: new[] { "PaypalTransactions" }, Summary = "Complete the transaction and create the donation.", Description = "This will capture the transaction, create a donation and update the project.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DonationDTO), Required = true, Description = "Donation object for donation details")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Created, Summary = "Transaction Completed", Description = "Transaction Completed")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> CompleteTransaction1([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "transactions/paypal/complete1")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DonationDTO donationDTO = JsonConvert.DeserializeObject<DonationDTO>(requestBody);
            await _transactionService.CompleteTransaction1(donationDTO);
            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteStringAsync("Transaction done successfully!");
            return response;

        }
    }
}

