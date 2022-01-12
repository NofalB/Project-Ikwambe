using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
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
        public Guid? UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the project ID.")]
        public Guid ProjectId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction ID.")]
        public string TransactionId { get; set; }

        /*[OpenApiProperty(Description = "Gets or sets the amount.")]
        public double Amount { get; set; }*/

        //update for the front end
        [OpenApiProperty(Description = "Gets or sets the input comment of the donor.")]
        public string Comment { get; set; }

        [OpenApiProperty(Description = "Gets or sets the input name of the anonymous donor.")]
        public string Name { get; set; }
        //update for the front end

        /*[OpenApiProperty(Description = "Gets or sets the date the donation was made.")]
        public DateTime DonationDate { get; set; }*/

        public DonationDTO()
        {
            //DonationDate = DateTime.Now;
        }

        public DonationDTO(Guid userId, Guid projectId, string transactionId, /*double amount,*/ string comment, string name)
        {
            UserId = userId;
            ProjectId = projectId;
            TransactionId = transactionId;
            //Amount = amount;
            Comment = comment;
            Name = name;
            //DonationDate = DateTime.Now;
        }
    }
    public class DummyDonationDTOExample : OpenApiExample<DonationDTO>
    {
        public override IOpenApiExample<DonationDTO> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Donation 1", new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000, "This is a comment", "Anonnymous donator"), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Donation 2", new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 599, "For the good cause", "Anonnymous donator"), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Donation 3", new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 200, "Donation from my family", "Franklin George"), NamingStrategy));

            return this;
        }
    }

    public class DummyDonationsDTOExamples : OpenApiExample<List<DonationDTO>>
    {
        public override IOpenApiExample<List<DonationDTO>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTOs", new List<DonationDTO> {
                    new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", /*4000,*/ "Donation from my family", "Franklin George"),
                    new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", /*599,*/ "Donation from my family", "Franklin George"),
                    new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", /*200,*/ "Donation from my family", "Franklin George")
                }));

            return this;
        }
    }
}
