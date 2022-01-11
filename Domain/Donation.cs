using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class Donation
    {
        [OpenApiProperty(Description = "Gets or sets the donation ID.")]
        public Guid DonationId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user ID.")]
        public Guid? UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the project ID.")]
        public Guid ProjectId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction ID.")]
        public string TransactionId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the amount.")]
        public double Amount { get; set; }

        [OpenApiProperty(Description = "Gets or sets the date the donation was made.")]
        public DateTime DonationDate { get; set; }

        public string PartitionKey { get; set; }// equals project id

        public Donation()
        {

        }

        public Donation(Guid donationId, Guid userId, Guid projectId, string transactionId, double amount)
        {
            DonationId = donationId;
            UserId = userId;
            ProjectId = projectId;
            TransactionId = transactionId;
            Amount = amount;
            DonationDate = DateTime.Now;
        }
    }

    public class DummyDonationExample : OpenApiExample<Donation>
    {
        public override IOpenApiExample<Donation> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Donation 1", new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Donation 2", new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 599), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Donation 3", new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 200), NamingStrategy));

            return this;
        }
    }

    public class DummyDonationsExamples : OpenApiExample<List<Donation>>
    {
        public override IOpenApiExample<List<Donation>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Donations", new List<Donation> {
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000),
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 599),
                    new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 200)
                }));

            return this;
        }
    }
}
