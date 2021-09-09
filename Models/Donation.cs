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

        public double Amount { get; set; }

        public bool Recurring { get; set; }

        public Donation()
        {

        }

        public Donation(int donationId, int userId, double amount, bool recurring)
        {
            DonationId = donationId;
            UserId = userId;
            Amount = amount;
            Recurring = recurring;
        }
    }

    public class DummyDonationExample : OpenApiExample<Donation>
    {
        public override IOpenApiExample<Donation> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Example1", new Donation(421, 23, 4000, false), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Example2", new Donation(521, 12, 599, true), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Example3", new Donation(631, 3, 300, true), NamingStrategy));

            return this;
        }
    }

    public class DummyDonationsExamples : OpenApiExample<List<Donation>>
    {
        public override IOpenApiExample<List<Donation>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Pets", new List<Donation> {
                    new Donation(421, 23, 4000, false),
                    new Donation(521, 12, 599, true),
                    new Donation(631, 3, 300, true)
                }));

            return this;
        }
    }
}
