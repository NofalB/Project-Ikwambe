using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Models
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
        public TransactionLink Links { get; set; }



    }
}
