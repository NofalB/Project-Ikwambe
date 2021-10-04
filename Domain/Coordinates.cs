using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Device.Location;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class Coordinates
    {
        public string CoordinateId { get; set; }

        public string LocationName { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
        public Coordinates()
        {

        }

        public Coordinates(string coordinateId, string locationName, double longitude, double latitude)
        {
			CoordinateId = coordinateId;
			LocationName = locationName;
            Longitude = longitude;
            Latitude = latitude;
        }
        //GeoCoordinate(-8.000, 36.833330)
    }

    public class DummyCoordinatesExample : OpenApiExample<Coordinates>
    {
		public override IOpenApiExample<Coordinates> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("coordinates", new Coordinates()
			{
				CoordinateId = "1",
                LocationName = "Ikwambe",
                Longitude = -8.000,
                Latitude = 36.833330
            }));

			return this;
		}
	}

    public class DummyCoordinatesExamples : OpenApiExample<List<Coordinates>>
    {
        public override IOpenApiExample<List<Coordinates>> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("coordinates", new List<Coordinates> {
                    new Coordinates("1", "Ikwambe", -8.000, 36.833330),
                    new Coordinates("2", "Haarlem", -1.000, 69.833330)

            }));
            return this;
        }
    }

}
