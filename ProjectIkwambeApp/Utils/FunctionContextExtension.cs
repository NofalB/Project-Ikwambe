using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Http;

namespace ProjectIkwambe.Utils
{
    public static class FunctionContextExtension
    {
		public static ClaimsPrincipal GetUser(this FunctionContext FunctionContext)
		{
			try
			{
				return (ClaimsPrincipal)FunctionContext.Items["User"];
			}
			catch (Exception e)
			{
				throw new UnauthorizedAccessException(/*e.Message*/);
			}
		}

		public static ClaimsPrincipal GetAdmin(this FunctionContext FunctionContext)
		{
			try
			{
				return (ClaimsPrincipal)FunctionContext.Items["Admin"];
			}
			catch (Exception e)
			{
				throw new UnauthorizedAccessException(/*e.Message*/);
			}
		}

		public static HttpRequestData GetHttpRequestData(this FunctionContext context)
		{
			var keyValuePair = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
			var functionBindingsFeature = keyValuePair.Value;
			var type = functionBindingsFeature.GetType();
			var inputData = type.GetProperties().Single(p => p.Name == "InputData").GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
			return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
		}

		public static void InvokeResult(this FunctionContext context, HttpResponseData response)
		{
			var keyValuePair = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
			var functionBindingsFeature = keyValuePair.Value;
			var type = functionBindingsFeature.GetType();
			var result = type.GetProperties().Single(p => p.Name == "InvocationResult");
			result.SetValue(functionBindingsFeature, response);
		}
	}
}
