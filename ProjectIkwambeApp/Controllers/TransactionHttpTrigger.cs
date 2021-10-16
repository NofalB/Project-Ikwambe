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
        HttpClient httpClient = new HttpClient();
        public TransactionHttpTrigger(ILogger<TransactionHttpTrigger> Logger, ITransactionService transactionService, IPaypalClientService paypalClientService)
		{
			this.Logger = Logger;
            _transactionService = transactionService;
            _paypalClientService = paypalClientService;

        }

        [Function(nameof(TransactionHttpTrigger.GetTransactions))]
        [OpenApiOperation(tags: new[] { "DBTransactions" }, Summary = "Get all transactions from db", Description = "This will retrieve all transactions", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        public async Task<HttpResponseData> GetTransactions([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "dbtransactions")] HttpRequestData req, FunctionContext executionContext)
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
        public async Task<HttpResponseData> GetTransactionsById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "dbtransactions/{transactionId}")] HttpRequestData req, FunctionContext executionContext)
        {
            string transactionId = HttpUtility.ParseQueryString(req.Url.Query).Get("transactionId");

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _transactionService.GetTransactionById(transactionId));
            return response;
            
        }

        [Function(nameof(TransactionHttpTrigger.GetTransactionsPayPal))]
        [OpenApiOperation(tags: new[] { "PaypalTransactions" }, Summary = "Get a transaction directly from paypal microservice", Description = "This will retrieve a transaction directly from paypal microservice", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        public async Task<HttpResponseData> GetTransactionsPayPal([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/{transactionId}")] HttpRequestData req, FunctionContext executionContext)
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
		public async Task<HttpResponseData> CreateCheckoutUrl([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactionsurl")] HttpRequestData req, FunctionContext executionContext)
		{
            string currencyCode = HttpUtility.ParseQueryString(req.Url.Query).Get("currency");
            string value = HttpUtility.ParseQueryString(req.Url.Query).Get("value");

            var checkoutUrl =await _paypalClientService.GetCheckoutUrl(currencyCode, value);
            
            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(checkoutUrl);
            return response;
         
        }

        [Function(nameof(TransactionHttpTrigger.CompleteTransaction))]
        [OpenApiOperation(operationId: "transaction", tags: new[] { "PaypalTransactions" }, Summary = "Complete the transaction and create the donation.", Description = "This will capture the transaction, create a donation and update the project.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CheckoutUrl), Summary = "New transaction details", Description = "New transaction details")]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "projectId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of project to donate to", Description = "Donates to this project", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> CompleteTransaction([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/complete")] HttpRequestData req, FunctionContext executionContext)
        {
            string transactionId = HttpUtility.ParseQueryString(req.Url.Query).Get("transactionId");
            string projectId = HttpUtility.ParseQueryString(req.Url.Query).Get("projectId");

            //get this from user who logs in
            //project id we get from the query param
            //transaction id is got from the frontend when payment is made
            Guid guid = new Guid();
            await _transactionService.CompleteTransaction(transactionId,guid,projectId);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            
            return response;

        }

        [Function(nameof(TransactionHttpTrigger.AddTransaction))]
        [OpenApiOperation(operationId: "transaction", tags: new[] { "PaypalTransactions" }, Summary = "Create a transaction URL", Description = "This will create a transaction link", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CheckoutUrl), Summary = "New transaction details", Description = "New transaction details")]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> AddTransaction([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "transactions/ADD")] HttpRequestData req, FunctionContext executionContext)
        {
            string transactionId = HttpUtility.ParseQueryString(req.Url.Query).Get("transactionId");
            var transaction = await _paypalClientService.GetTransaction(transactionId);
            await _transactionService.AddTransaction(transaction);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(transaction);
            return response;


        }
    }
}

