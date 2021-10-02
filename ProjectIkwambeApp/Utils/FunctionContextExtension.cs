﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using System.Security.Claims;

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
	}
}