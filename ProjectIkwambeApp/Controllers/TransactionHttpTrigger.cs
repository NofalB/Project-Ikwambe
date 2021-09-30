using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain;
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

		public TransactionHttpTrigger(ILogger<TransactionHttpTrigger> Logger)
		{
			this.Logger = Logger;
		}

		[Function(nameof(TransactionHttpTrigger.GetTransactions))]
		[OpenApiOperation(tags: new[] { "Transactions" }, Summary = "Get all transactions", Description = "This will retrieve all transactions", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "transactionId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "ID of transaction to return", Description = "Retrieves a specific transaction by ID", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Successfully fetched transactions", Description = "transactions successfully retrieved", Example = typeof(Transaction.DummyTransactionsExamples))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid transaction ID", Description = "Invalid transaction ID was provided")]
		public async Task<HttpResponseData> GetTransactions([HttpTrigger(AuthorizationLevel.Function, "GET", Route = "transactions")] HttpRequestData req, FunctionContext executionContext)
		{
			{
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
				TransactionAmount transactionAmount = new TransactionAmount("EUR", "54.00");
				TransactionLink transactionLink = new("https://api.mollie.com/v2/payments/tr_WDqYK6vllg", "https://www.mollie.com/payscreen/select-method/WDqYK6vllg");

				List<Transaction> transactions = new List<Transaction>()
				{
					new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Paypal",transactionLink),
					new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Banq",transactionLink),
					new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Ideal",transactionLink)
				};

				await response.WriteAsJsonAsync(transactions);

				return response;
			}
		}

		[Function(nameof(TransactionHttpTrigger.MakeTransactions))]
		[OpenApiOperation(operationId: "transaction", tags: new[] { "Transactions" }, Summary = "Make a transaction", Description = "This will create a transaction", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Transaction), Required = true, Description = "Transaction object for transaction details")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "New transaction details", Description = "New transaction details", Example = typeof(Transaction.DummyTransactionExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> MakeTransactions([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "transactions")] HttpRequestData req, FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Transaction transaction = JsonConvert.DeserializeObject<Transaction>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(transaction);

			return response;
		}
	}
}

