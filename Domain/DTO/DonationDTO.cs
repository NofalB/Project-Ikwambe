﻿using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class DonationDTO
    {
        [Column(TypeName = "Id")]
        public string UserForeignKey { get; set; }

        [OpenApiProperty(Description = "Gets or sets the project ID.")]
        public string ProjectId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction ID.")]
        public string TransactionId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the amount.")]
        public double Amount { get; set; }

        [OpenApiProperty(Description = "Gets or sets the date the donation was made.")]
        public DateTime DonationDate { get; set; }

        public string PartitionKey { get; set; }

        public DonationDTO()
        {

        }

        public DonationDTO(string donationId, /*User userId,*/ string projectId, string transactionId, double amount)
        {
            //User = userId;
            ProjectId = projectId;
            TransactionId = transactionId;
            Amount = amount;
            DonationDate = DateTime.Now;
        }
    }
    public class DummyDonationDTOExample : OpenApiExample<DonationDTO>
    {
        public override IOpenApiExample<DonationDTO> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTO 1", new DonationDTO("421", /*new User(100, "Kratos", "Jumbo", "bruh@gmail.com", "380", true),*/ "4", "24", 4000), NamingStrategy));
            //Examples.Add(OpenApiExampleResolver.Resolve("Donation 2", new Donation(521, "12", "10", "12", 599), NamingStrategy));
            //Examples.Add(OpenApiExampleResolver.Resolve("Donation 3", new Donation(631, "3", "30", "7", 200), NamingStrategy));

            return this;
        }
    }

    public class DummyDonationsDTOExamples : OpenApiExample<List<DonationDTO>>
    {
        public override IOpenApiExample<List<DonationDTO>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTOs", new List<DonationDTO> {
                    new DonationDTO("421", /*new User(100, "Kratos", "Jumbo", "bruh@gmail.com", "380", true),*/ "4", "24", 4000),
                    new DonationDTO("521", /*new User(100, "Kratos", "Jumbo", "bruh@gmail.com", "380", true),*/ "10", "12", 599),
                    new DonationDTO("631", /*new User(100, "Kratos", "Jumbo", "bruh@gmail.com", "380", true),*/ "30", "7", 200)
                }));

            return this;
        }
    }
}