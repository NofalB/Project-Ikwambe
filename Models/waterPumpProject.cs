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
					//Latitude = -8.000, Longitude = 36.833330
					Coordination = new System.Device.Location.GeoCoordinate(-8.000, 36.833330),
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
				new WaterPumpProject() { Id = 1, NameOfProject = "waterPump Ikwambe", Location = "Village of Ikwambe", Coordination = new System.Device.Location.GeoCoordinate(-8.000, 36.833330), CurrentDonation = 0, TargetGoal = 25000, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20},
				new WaterPumpProject() { Id = 2, NameOfProject = "waterPumpAlmere", Location = "Almere", Coordination = new System.Device.Location.GeoCoordinate(-8.000, 36.833330), CurrentDonation = 123, TargetGoal = 40000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50},
				new WaterPumpProject() { Id = 3, NameOfProject = "waterPumpAmsterdam", Location = "Amsterdam", Coordination = new System.Device.Location.GeoCoordinate(-8.000, 36.833330), CurrentDonation = 456, TargetGoal = 66000, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 50, FlowRate = 200}
			}));
				
				return this;
			}
		}
	}
}
