﻿using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Device.Location;

namespace ProjectIkwambe.Models
{
	public abstract class Project
	{
		[OpenApiProperty(Description = "Get or sets  of the ID")]
		public long? Id { get; set; }

		[OpenApiProperty(Description = "The name of the project")]
		[JsonRequired]
		public string NameOfProject { get; set; }

		[OpenApiProperty(Description = "Get or sets the location of the water pump")]
		[JsonRequired]
		public string Location { get; set; }

		[OpenApiProperty(Description = "Get or sets the Coordination of the water pump")]
		[JsonRequired]
		public GeoCoordinate Coordination { get; set; }

		[OpenApiProperty(Description = "The current amount collected from all the donations")]
		[JsonRequired]
		public double? CurrentDonation { get; set; }

		[OpenApiProperty(Description = "Get or sets the target goal to achieve the amount needed to finish the project")]
		[JsonRequired]
		public double? TargetGoal { get; set; }

		[OpenApiProperty(Description = "Get or sets the date when the project started.")]
		[JsonRequired]
		public DateTime? startDate { get; set; }

		[OpenApiProperty(Description = "Get or sets the date when the project will end.")]
		[JsonRequired]
		public DateTime? EndDate { get; set; }

	}
}
