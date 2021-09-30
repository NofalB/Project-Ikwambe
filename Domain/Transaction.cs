using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Transaction
    {
        [OpenApiProperty(Description = "Gets or sets the a response containing a payment object. Will always contain payment for this endpoint.")]
        [JsonRequired]
        public string Resource { get; set; }

        [OpenApiProperty(Description = "Gets or sets a unique transaction id for a transaction")]
        [JsonRequired]
        public string TransactionId { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the amount object for a transaction.")]
        [JsonRequired]
        public TransactionAmount Amount { get; set; }

        [OpenApiProperty(Description = "Gets or sets a short description of the payment. ")]
        [JsonRequired]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the URL your customer will be redirected to after completing or canceling the payment process.")]
        [JsonRequired]
        public string RedirectUrl { get; set; }

        [OpenApiProperty(Description = "Gets or sets the mode(real or test) used to create this payment")]
        [JsonRequired]
        public string Mode { get; set; }

        [OpenApiProperty(Description = "Gets or sets the payment’s date and time of creation, in ISO 8601 format.")]
        [JsonRequired]
        public DateTime CreatedAt { get; set; }

        [OpenApiProperty(Description = "Gets or sets the payment’s status.")]
        [JsonRequired]
        public string Status { get; set; }

        [OpenApiProperty(Description = "Gets or sets the payment method used for this payment, either forced on creation by specifying the method parameter, or chosen by the customer on our payment method selection screen.")]
        [JsonRequired]
        public string Method { get; set; }

        [OpenApiProperty(Description = "Gets or sets an object with several URL objects relevant to the payment. Every URL object will contain an href and a type field.")]
        [JsonRequired]
        public TransactionLink Link { get; set; }

        public Transaction(string resource,string transactionId,TransactionAmount amount,string description,string redirectUrl,string mode,DateTime createdAt, string status,string method,TransactionLink link)
        {
            Resource = resource;
            TransactionId = transactionId;
            Amount = amount;
            Description = description;
            RedirectUrl = redirectUrl;
            Mode = mode;
            CreatedAt = createdAt;
            Status = status;
            Method = method;
            Link = link;
        }

        public class DummyTransactionExample : OpenApiExample<Transaction>
        {
            public override IOpenApiExample<Transaction> Build(NamingStrategy NamingStrategy = null)
            {
                TransactionAmount transactionAmount = new TransactionAmount("EUR", "10.00");
                TransactionLink transactionLink = new("https://api.mollie.com/v2/payments/tr_WDqYK6vllg", "https://www.mollie.com/payscreen/select-method/WDqYK6vllg");

                Examples.Add(OpenApiExampleResolver.Resolve("Transaction 1", new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Paypal",transactionLink), NamingStrategy));
                Examples.Add(OpenApiExampleResolver.Resolve("Transaction 2", new Transaction("payment", "tr_WDqYK6vllg", transactionAmount, "Donation #676", "https://webshop.example.org/order/12345/", "live", DateTime.Now, "open", "Banq", transactionLink), NamingStrategy));
                Examples.Add(OpenApiExampleResolver.Resolve("Transaction 3", new Transaction("payment", "tr_WDqYK6vllg", transactionAmount, "Donation #234", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Ideal", transactionLink), NamingStrategy));

                return this;
            }
        }

        public class DummyTransactionsExamples : OpenApiExample<List<Transaction>>
        {
             TransactionAmount transactionAmount = new TransactionAmount("EUR", "54.00");
             TransactionLink transactionLink = new("https://api.mollie.com/v2/payments/tr_WDqYK6vllg", "https://www.mollie.com/payscreen/select-method/WDqYK6vllg");
            public override IOpenApiExample<List<Transaction>> Build(NamingStrategy NamingStrategy = null)
            {
                Examples.Add(OpenApiExampleResolver.Resolve("Transactions", new List<Transaction> {
                    new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Paypal",transactionLink),
                    new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Banq",transactionLink),
                    new Transaction("payment", "tr_WDqYK6vllg",transactionAmount, "Donation #12345", "https://webshop.example.org/order/12345/", "test", DateTime.Now, "open", "Ideal",transactionLink)
                }));

                return this;
            }
        }

    }
}
