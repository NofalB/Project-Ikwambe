using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectType
    {
        [EnumMember(Value = "Health_service")]
        Health_service = 0,

        [EnumMember(Value = "Education")]
        Education = 1,

        [EnumMember(Value = "Infrastructrure")]
        Infrastructrure = 2
    }
}
