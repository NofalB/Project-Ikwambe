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
        public Guid TransactionId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the amount.")]
        public double Amount { get; set; }

        [OpenApiProperty(Description = "Gets or sets the date the donation was made.")]
        public DateTime DonationDate { get; set; }

        public DonationDTO()
        {

        }

        public DonationDTO(Guid userId, Guid projectId, Guid transactionId, double amount)
        {
            UserId = userId;
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
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTO 1", new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 4000), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTO 2", new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 599), NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTO 3", new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 200), NamingStrategy));

            return this;
        }
    }

    public class DummyDonationsDTOExamples : OpenApiExample<List<DonationDTO>>
    {
        public override IOpenApiExample<List<DonationDTO>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("DonationDTOs", new List<DonationDTO> {
                    new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 4000),
                    new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 599),
                    new DonationDTO(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 200)
                }));

            return this;
        }
    }
}
