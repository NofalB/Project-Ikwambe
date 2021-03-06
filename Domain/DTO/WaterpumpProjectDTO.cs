using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
	public class WaterpumpProjectDTO
	{
		[OpenApiProperty(Description = "The name of the project")]
		[JsonRequired]
		public string NameOfProject { get; set; }

		public string Description { get; set; }

		[OpenApiProperty(Description = "Get or sets the Coordination of the project")]
		[JsonRequired]
		public Coordinates Coordinates { get; set; }

		[OpenApiProperty(Description = "Get or sets the target goal to achieve the amount needed to finish the project")]
		[JsonRequired]
		public double TargetGoal { get; set; }

		[OpenApiProperty(Description = "Get or sets the date when the project started.")]
		[JsonRequired]
		public DateTime StartDate { get; set; }

		[OpenApiProperty(Description = "Get or sets the date when the project will end.")]
		[JsonRequired]
		public DateTime EndDate { get; set; }

		[OpenApiProperty(Description = "Get or sets the number of the donators.")]
		[JsonRequired]
		public int TotalNumbOfDonators { get; set; }

		[OpenApiProperty(Description = "The maximum power input needed for the waterpump")]
		[JsonRequired]
		public int RatedPower { get; set; }

		[OpenApiProperty(Description = "The pump speed of the device")]
		[JsonRequired]
		public int FlowRate { get; set; }

		public ProjectType ProjectType { get; set; }

		public WaterpumpProjectDTO()
		{

		}

		public WaterpumpProjectDTO(string nameOfProject, string description, Coordinates coordinates, double targetGoal, DateTime startDate, DateTime endDate, int totalNumbOfDonors, int ratedPower, int flowRate, ProjectType projectType)
		{
			NameOfProject = nameOfProject;
			Description = description;
			Coordinates = coordinates;
			TargetGoal = targetGoal;
			StartDate = startDate;
			EndDate = endDate;
			TotalNumbOfDonators = totalNumbOfDonors;
			RatedPower = ratedPower;
			FlowRate = flowRate;
			ProjectType = projectType;
		}

	}

	public class DummyWaterpumpProjectDTOExample : OpenApiExample<WaterpumpProjectDTO>
	{
		public override IOpenApiExample<WaterpumpProjectDTO> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("WaterpumpResult", new WaterpumpProjectDTO(
				"WaterpumpIkwambe",
				"This is a description",
				new Coordinates("ikwambe", -8.000, 36.833330), 
				//0, 
				25000, 
				DateTime.Now, 
				DateTime.Now,
				0,
				650, 
				200,
				ProjectType.infrastructure),
				NamingStrategy));
			
			return this;
		}
	}

	public class DummyWaterpumpProjectDTOExamples : OpenApiExample<List<WaterpumpProjectDTO>>
	{
		public override IOpenApiExample<List<WaterpumpProjectDTO>> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Waterpumps", new List<WaterpumpProjectDTO> {
				new WaterpumpProjectDTO("waterPumpIkwambe", "This is a description", new Coordinates("ikwambe", -8.000, 36.833330), 25000, DateTime.Now, DateTime.Now, 1, 20, 20, ProjectType.infrastructure),
				new WaterpumpProjectDTO("waterPumpAlmere", "This is a description", new Coordinates("ikwambe", -8.000, 36.833330), 40000, DateTime.Now, DateTime.Now, 2, 100, 50, ProjectType.infrastructure),
				new WaterpumpProjectDTO("waterPumpAmsterdam", "This is a description", new Coordinates( "ikwambe", -8.000, 36.833330), 66000, DateTime.Now, DateTime.Now, 3, 50, 200, ProjectType.education)
			}));

			return this;
		}
	}
}
