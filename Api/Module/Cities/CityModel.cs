using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Api.Cities
{
	public class CityModel
	{
		public int id { get; set; }
		public int countryId { get; set; }
		public string name { get; set; }
		public string nameInvariant { get; set; }
	}
}