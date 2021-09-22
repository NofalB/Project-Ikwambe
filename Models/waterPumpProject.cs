using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProjectIkwambe.Models
{
    public class waterPumpProject : Project
    {
		[OpenApiProperty(Description = "The maximum power input needed for the waterpump")]
		[JsonRequired]
		public int ratedPower { get; set; }

		[OpenApiProperty(Description = "The pump speed of the device")]
		[JsonRequired]
		public int FlowRate { get; set; }

		public class DummyWaterPumpProjectExample : OpenApiExample<waterPumpProject>
		{
			public override IOpenApiExample<waterPumpProject> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("waterPumpIkwambe", new waterPumpProject()
				{
					Id = 1,
					nameOfProject = "waterPumpIkwambe",
					location = "Village of Ikwambe",
					currentDonation = 0,
					targetGoal = 25000,
					startDate = DateTime.Now,
					endDate = DateTime.Now,
					ratedPower = 650,
					FlowRate = 200,
				}));

				return this;
			}
		}


		public class DummyWaterPumpProjectExamples : OpenApiExample<List<waterPumpProject>>
		{
			public override IOpenApiExample<List<waterPumpProject>> Build(NamingStrategy NamingStrategy = null)
			{
				Examples.Add(OpenApiExampleResolver.Resolve("waterPumps", new List<waterPumpProject> {
				new waterPumpProject() { Id = 1, nameOfProject = "waterPump Ikwambe", location = "Village of Ikwambe", currentDonation = 0, targetGoal = 25000, startDate = DateTime.Now, endDate = DateTime.Now , ratedPower = 20, FlowRate = 20},
				new waterPumpProject() { Id = 2, nameOfProject = "waterPumpAlmere", location = "Almere", currentDonation = 123, targetGoal = 40000, startDate = DateTime.Now, endDate = DateTime.Now, ratedPower = 100, FlowRate = 50},
				new waterPumpProject() { Id = 3, nameOfProject = "waterPumpAmsterdam", location = "Amsterdam", currentDonation = 456, targetGoal = 66000, startDate = DateTime.Now, endDate = DateTime.Now, ratedPower = 50, FlowRate = 200}
			}));
				
				return this;
			}
		}
	}
}
