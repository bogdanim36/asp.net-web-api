using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using WebApi.Api.Base;

namespace WebApi.Api.Countries

{
    public class CountryController : ApiController
    {
		Country country = new Country();

		[HttpGet]
		[AllowAnonymous]
		[Route("countries")]
		public ActionResponse Get()
		{
			var response = new ActionResponse();
			response.data = country.getItems();
			response.status = true;
			return response;
		}
		[HttpGet]
		[AllowAnonymous]
		[Route("countries/{id}")]
		public ActionResponse Get( int id)
		{
			var response = new ActionResponse();
			response.data = country.getItem(id);
			response.status = true;
			return response;
		}

		[HttpPut]
		[AllowAnonymous]
		[Route("countries/{id}")]
		public ActionResponse Put(int id, [FromBody] dynamic bodyParams)
		{
			var response = new ActionResponse();
			JObject item = bodyParams.item as JObject;
			CountryModel countryData = item.ToObject<CountryModel>();
			response.data = country.Update(id, countryData);
			response.status = true;
			return response;
		}
	}
}
