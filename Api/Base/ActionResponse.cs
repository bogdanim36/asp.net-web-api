using System;
using System.Collections.Generic;

namespace WebApi.Api.Base
{
	public class ActionResponse
	{
		public Boolean status { get; set; }
		public dynamic data { get; set; }
		public Dictionary<string, List<string>> errors { get; set; }
		public void addError(string label, string msg)
		{
			if (errors == null || errors.Count == 0) errors = new Dictionary<string, List<string>>();
			List<string> msgList;
			if (errors.ContainsKey(label))
			{
				errors.TryGetValue(label, out msgList);
				msgList.Add(msg);
			}
			else
			{
				msgList = new List<string>();
				msgList.Add(msg);
				errors.Add(label, msgList);
			}
		}
	}
}