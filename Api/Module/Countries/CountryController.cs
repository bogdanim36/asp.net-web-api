using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using WebApi.Api.Base;

namespace WebApi.Api.Countries
{
	public class CountryController : ApiController
	{
		Country service = new Country();

		[HttpGet]
		[AllowAnonymous]
		[Route("countries")]
		public ActionResponse Get()
		{
			var response = new ActionResponse();
			response.data = service.getItems();
			response.status = true;
			return response;
		}
		[HttpGet]
		[AllowAnonymous]
		[Route("countries/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = service.getItem(id);
			response.status = true;
			return response;
		}

		[HttpPut]
		[AllowAnonymous]
		[Route("countries/{id}")]
		public ActionResponse Put(int id, [FromBody] dynamic bodyParams)
		{
			var response = new ActionResponse();
			try
			{
				JObject item = bodyParams.item as JObject;
				CountryModel countryData = item.ToObject<CountryModel>();
				response.data = service.Update(id, countryData);
				response.status = true;
			}
			catch (Exception ex)
			{
				response.addError("Exception", ex.GetBaseException().Message);
				response.addError("Exception", ex.GetBaseException().StackTrace);
				response.status = false ;
			}
			return response;
		}
	}
}
