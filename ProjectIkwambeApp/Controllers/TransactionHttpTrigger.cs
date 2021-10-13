using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Domain;
using Infrastructure.Services;
using Infrastructure.Services.Clients;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

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

        [Function(nameof(TransactionHttpTrigger.GetTransactions))]
        [OpenApiOperation(tags: new[] { "DBTransactions" }, Summary = "Get all transactions from db", Description = "This will retrieve all transactions", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        public async Task<HttpResponseData> GetTransactions([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "dbtransactions")] HttpRequestData req, FunctionContext executionContext)
        {
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(await _transactionService.GetAllTransactions());

                return response;
            }
        }

        [Function(nameof(TransactionHttpTrigger.GetTransactionsById))]
        [OpenApiOperation(tags: new[] { "DBTransactions" }, Summary = "Get a specific transactions from db", Description = "This will retrieve a specific transaction from db", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid transaction ID", Description = "Invalid transaction ID was provided")]
        public async Task<HttpResponseData> GetTransactionsById([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "dbtransactions/{transactionId}")] HttpRequestData req, FunctionContext executionContext)
        {
            {
                string transactionId = HttpUtility.ParseQueryString(req.Url.Query).Get("transactionId");

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(await _transactionService.GetTransactionById(transactionId));
                return response;
            }
        }

        [Function(nameof(TransactionHttpTrigger.GetTransactionsPayPal))]
        [OpenApiOperation(tags: new[] { "PaypalTransactions" }, Summary = "Get a transaction directly from paypal microservice", Description = "This will retrieve a transaction directly from paypal microservice", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        public async Task<HttpResponseData> GetTransactionsPayPal([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "transactions/{transactionId}")] HttpRequestData req, FunctionContext executionContext)
        {
            string transactionId = HttpUtility.ParseQueryString(req.Url.Query).Get("transactionId");
            var transaction = await _paypalClientService.GetTransaction(transactionId);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transaction);
            return response;
        }

        [Function(nameof(TransactionHttpTrigger.CreateCheckoutUrl))]
		[OpenApiOperation(operationId: "transaction", tags: new[] { "PaypalTransactions" }, Summary = "Create a transaction URL", Description = "This will create a transaction link", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CheckoutUrl), Summary = "New transaction details", Description = "New transaction details")]
		[OpenApiParameter(name: "currency", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The currency required for the transaction creation", Description = "Creates the transaction with specified currency type", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "value", In = ParameterLocation.Query, Required = true, Type = typeof(int), Summary = "The value/amount required for the transaction creation", Description = "Creates the transaction with specified value amount", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> CreateCheckoutUrl([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "transactionsurl")] HttpRequestData req, FunctionContext executionContext)
		{
            string currencyCode = HttpUtility.ParseQueryString(req.Url.Query).Get("currency");
            string value = HttpUtility.ParseQueryString(req.Url.Query).Get("value");

            var checkoutUrl =await _paypalClientService.GetCheckoutUrl(currencyCode, value);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(checkoutUrl);
            return response;
         
        }
    }
}

