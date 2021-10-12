using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Domain;
using Infrastructure.Services;
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
		private static HttpClient httpClient = new HttpClient();
        private ITransactionService _transactionService;

		public TransactionHttpTrigger(ILogger<TransactionHttpTrigger> Logger, ITransactionService transactionService)
		{
			this.Logger = Logger;
            _transactionService = transactionService;

        }

        [Function(nameof(TransactionHttpTrigger.GetTransactions))]
        [OpenApiOperation(tags: new[] { "Transactions" }, Summary = "Get all transactions from db", Description = "This will retrieve all transactions", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid transaction ID", Description = "Invalid transaction ID was provided")]
        public async Task<HttpResponseData> GetTransactions([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "transactions")] HttpRequestData req, FunctionContext executionContext)
        {
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(await _transactionService.GetAllTransactions());

                return response;
            }
        }

        [Function(nameof(TransactionHttpTrigger.CreateTransactions))]
		[OpenApiOperation(operationId: "transaction", tags: new[] { "Transactions" }, Summary = "Create a transaction URL", Description = "This will create a transaction link", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CreateTransaction), Summary = "New transaction details", Description = "New transaction details")]
		[OpenApiParameter(name: "currency", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The currency required for the transaction creation", Description = "Creates the transaction with specified currency type", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "value", In = ParameterLocation.Query, Required = true, Type = typeof(int), Summary = "The value/amount required for the transaction creation", Description = "Creates the transaction with specified value amount", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> CreateTransactions([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "transactions")] HttpRequestData req, FunctionContext executionContext)
		{
			string currencyCode = HttpUtility.ParseQueryString(req.Url.Query).Get("currency");
			string value = HttpUtility.ParseQueryString(req.Url.Query).Get("value");

			var TransactionData = await httpClient.GetAsync("https://paypalmicroserviceikwambe.azurewebsites.net/api/createorder?currency="+ currencyCode + "&value="+value);
			var TransactionDataResponseObj = await TransactionData.Content.ReadAsStringAsync();
			var TransactionDataObj = JsonConvert.DeserializeObject<CreateTransaction>(TransactionDataResponseObj);

            //currently doing it here to check the service and db for transactions
            //later needs to be done with vue only when it has been authorized
            //var TransactionResult = await httpClient.GetAsync("https://paypalmicroserviceikwambe.azurewebsites.net/api/getorder?orderId="+ TransactionDataObj.TransactionId);
            var TransactionResult = await httpClient.GetAsync("https://paypalmicroserviceikwambe.azurewebsites.net/api/getorder?orderId=2PY04402262190020");
            var TransactionResultResponseObj = await TransactionResult.Content.ReadAsStringAsync();
            var TransactionResultObj = JsonConvert.DeserializeObject<Transaction>(TransactionResultResponseObj);
            TransactionResultObj.TransactionId = TransactionResultObj.id;
            TransactionResultObj.PartitionKey = TransactionResultObj.id;

            await _transactionService.AddTransaction(TransactionResultObj);

            // Generate output
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(TransactionDataObj);

            return response;
        }
    }
}

