using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Donation
    {
        public int DonationId { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public int TransactionId { get; set; }

        public double Amount { get; set; }

        public DateTime DonationDate { get; set; }

        public Donation()
        {

        }

        public Donation(int donationId, int userId, double amount)
        {
            DonationId = donationId;
            UserId = userId;
            Amount = amount;
        }
    }

    public class DummyDonationExample : OpenApiExample<Donation>
    {
        public override IOpenApiExample<Donation> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Example1", new Donation(421, 23, 4000), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Example2", new Donation(521, 12, 599), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Example3", new Donation(631, 3, 300), NamingStrategy));

            return this;
        }
    }

    public class DummyDonationsExamples : OpenApiExample<List<Donation>>
    {
        public override IOpenApiExample<List<Donation>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Pets", new List<Donation> {
                    new Donation(421, 23, 4000),
                    new Donation(521, 12, 599),
                    new Donation(631, 3, 300)
                }));

            return this;
        }
    }
}
