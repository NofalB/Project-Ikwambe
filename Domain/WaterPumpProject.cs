using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain
{
    public class WaterpumpProject : Project
    {
		[OpenApiProperty(Description = "The maximum power input needed for the waterpump")]
		[JsonRequired]
		public int RatedPower { get; set; }

		[OpenApiProperty(Description = "The pump speed of the device")]
		[JsonRequired]
		public int FlowRate { get; set; }

		public WaterpumpProject()
        {

        }
	}

	public class DummyWaterpumpProjectExample : OpenApiExample<WaterpumpProject>
	{
		public override IOpenApiExample<WaterpumpProject> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("waterPumpIkwambe", new WaterpumpProject()
			{
				ProjectId = Guid.NewGuid(),
				NameOfProject = "waterpump Ikwambe",
				Description = "This is a description",
				Coordinates = new Coordinates("ikwambe", -8.000, 36.833330),
				CurrentTotal = 0,
				TargetGoal = 25000,
				TotalNumbOfDonators = 21,
				StartDate = DateTime.Now,
				EndDate = DateTime.Now,
				RatedPower = 650,
				FlowRate = 200,
				ProjectType = ProjectType.infrastructure
			}));
			return this;
		}
	}

	public class DummyWaterpumpProjectExamples : OpenApiExample<List<WaterpumpProject>>
	{
		public override IOpenApiExample<List<WaterpumpProject>> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("waterPumps", new List<WaterpumpProject> {
				new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPump Ikwambe",
				Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 0, TargetGoal = 25000, TotalNumbOfDonators = 1, StartDate = DateTime.Now, EndDate = DateTime.Now , RatedPower = 20, FlowRate = 20, ProjectType = ProjectType.infrastructure},
				new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAlmere",
				Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 123, TargetGoal = 40000,TotalNumbOfDonators = 2,  StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 100, FlowRate = 50, ProjectType = ProjectType.infrastructure},
				new WaterpumpProject() { ProjectId = Guid.NewGuid(), NameOfProject = "waterPumpAmsterdam",
				Coordinates = new Coordinates("ikwambe", -8.000, 36.833330), CurrentTotal = 456, TargetGoal = 66000, TotalNumbOfDonators = 3, StartDate = DateTime.Now, EndDate = DateTime.Now, RatedPower = 50, FlowRate = 200, ProjectType = ProjectType.infrastructure}
			}));
			return this;
		}
	}

}
