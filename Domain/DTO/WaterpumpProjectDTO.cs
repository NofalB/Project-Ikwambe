﻿using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
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
	public class WaterpumpProjectDTO : Project
	{
		[Key]
		[OpenApiProperty(Description = "The maximum power input needed for the waterpump")]
		[JsonRequired]
		public int RatedPower { get; set; }

		[OpenApiProperty(Description = "The pump speed of the device")]
		[JsonRequired]
		public int FlowRate { get; set; }

		public string PartitionKey { get; set; }


		public WaterpumpProjectDTO()
		{

		}

		public WaterpumpProjectDTO(string nameOfProject, Coordinates coordinates, double currentDonation, double targetGoal, DateTime startDate, DateTime endDate, int ratedPower, int flowRate)
		{
			NameOfProject = nameOfProject;
			Coordinates = coordinates;
			CurrentDonation = currentDonation;
			TargetGoal = targetGoal;
			StartDate = startDate;
			EndDate = endDate;
			RatedPower = ratedPower;
			FlowRate = flowRate;
		}

	}

	public class DummyWaterpumpProjectDTOExample : OpenApiExample<WaterpumpProjectDTO>
	{
		public override IOpenApiExample<WaterpumpProjectDTO> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("WaterpumpResult", new WaterpumpProjectDTO(
				"WaterpumpIkwambe", 
				new Coordinates("ikwambe", -8.000, 36.833330), 
				0, 
				25000, 
				DateTime.Now, 
				DateTime.Now, 
				650, 
				200),
				NamingStrategy));
			
			return this;
		}
	}

	public class DummyWaterpumpProjectDTOExamples : OpenApiExample<List<WaterpumpProjectDTO>>
	{
		public override IOpenApiExample<List<WaterpumpProjectDTO>> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Waterpumps", new List<WaterpumpProjectDTO> {
				new WaterpumpProjectDTO("waterPumpIkwambe", new Coordinates("ikwambe", -8.000, 36.833330), 0, 25000, DateTime.Now, DateTime.Now, 20, 20),
				new WaterpumpProjectDTO("waterPumpAlmere", new Coordinates("ikwambe", -8.000, 36.833330), 123, 40000, DateTime.Now, DateTime.Now, 100, 50),
				new WaterpumpProjectDTO("waterPumpAmsterdam", new Coordinates( "ikwambe", -8.000, 36.833330), 456, 66000, DateTime.Now, DateTime.Now, 50, 200)
			}));

			return this;
		}
	}
}