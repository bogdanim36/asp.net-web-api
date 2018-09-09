using System.Collections.Generic;
using WebApi.Api.Base;
using WebApi.Api.Cities;
using WebApi.Api.Countries;

namespace WebApi.Api.Customers
{
	public class GetCustomersResponse: ActionResponse
	{
		public IEnumerable<CountryModel> countries;
		public IEnumerable<CityModel> cities;
	}
}