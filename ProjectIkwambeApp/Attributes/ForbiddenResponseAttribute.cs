using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Attributes
{
    public class ForbiddenResponseAttribute : OpenApiResponseWithBodyAttribute
    {
        public ForbiddenResponseAttribute() : base(HttpStatusCode.Forbidden, "text/plain", typeof(string))
        {
            this.Description = "Your Account does not permit access to this function";
        }
    }
}
