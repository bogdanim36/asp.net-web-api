using System.Web.Http;
using WebApi.Api.Base;
using WebApi.Api.Countries;

namespace WebApi.Api.Cities

{
    public class CityController : ApiController
    {
		City city = new City();
		Country country = new Country();

		[HttpGet]
		[AllowAnonymous]
		[Route("cities")]
		public ActionResponse Get()
		{
			var response = new GetCitiesResponse();
			response.data = city.getItems();
			response.countries = country.getItems();
			response.status = true;
			return response;
		}
		[HttpGet]
		[AllowAnonymous]
		[Route("cities/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = city.getItem(id);
			response.status = true;
			return response;
		}

	}
}
