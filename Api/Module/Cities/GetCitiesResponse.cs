using System.Collections.Generic;
using WebApi.Api.Base;
using WebApi.Api.Countries;

namespace WebApi.Api.Cities
{
	public class GetCitiesResponse: ActionResponse
	{
		public IEnumerable<CountryModel> countries;
	}
}