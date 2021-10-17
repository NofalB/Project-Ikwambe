using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;

namespace ProjectIkwambe.Attributes
{
    public class UnauthorizedResponseAttribute : OpenApiResponseWithBodyAttribute
    {
        public UnauthorizedResponseAttribute() : base(HttpStatusCode.Unauthorized, "text/plain", typeof(string))
        {
            this.Description = "The User logged is invalid";
        }
    }
}
