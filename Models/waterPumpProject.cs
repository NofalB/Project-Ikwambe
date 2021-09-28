using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProjectIkwambe.Models
{
    public class WaterPumpProject : Project
    {
		[OpenApiProperty(Description = "The maximum power input needed for the waterpump")]
		[JsonRequired]
		public int RatedPower { get; set; }

		[OpenApiProperty(Description = "The pump speed of the device")]
		[JsonRequired]
		public int FlowRate { get; set; }

		public class DummyWaterPumpProjectExample : OpenApiExample<WaterPumpProject>
		{
			public override IOpenApiExample<WaterPumpProject> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("waterPumpIkwambe", new WaterPumpProject()
				{
					Id = 1,
					NameOfProject = "waterPumpIkwambe",
					Location = "Village of Ikwambe",
					//Coordination = 
					CurrentDonation = 0,
					TargetGoal = 25000,
					StartDate = DateTime.Now,
					EndDate = DateTime.Now,
					RatedPower = 650,
					FlowRate = 200,
				}));

				return this;
			}
		}


		public class DummyWaterPumpProjectExamples : OpenApiExample<List<WaterPumpProject>>
		{
			public override IOpenApiExample<List<WaterPumpProject>> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("waterPumps", new List<WaterPumpProject> {
				new WaterPumpProject() { Id = 1, NameOfProject = "waterPump Ikwambe", Location = "Village of Ikwambe", CurrentDonation = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20},
				new WaterPumpProject() { Id = 2, NameOfProject = "waterPumpAlmere", Location = "Almere", CurrentDonation = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50},
				new WaterPumpProject() { Id = 3, NameOfProject = "waterPumpAmsterdam", Location = "Amsterdam", CurrentDonation = 456, TargetGoal = 66000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 50, FlowRate = 200}
			}));
				
				return this;
			}
		}
	}
}
