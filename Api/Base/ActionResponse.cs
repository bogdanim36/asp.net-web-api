using System;
using System.Collections.Generic;

namespace WebApi.Api.Base
{
	public class ActionResponse
	{
		public Boolean status;
		public dynamic data;
		public Dictionary<string, List<string>> errors;
	}
}