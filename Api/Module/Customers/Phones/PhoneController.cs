using System.Web.Http;
using WebApi.Api.Base;

namespace WebApi.Api.Customers.Phones

{
	public class PhoneController : ApiController
	{
		Phone phones = new Phone();

		[HttpGet]
		[AllowAnonymous]
		[Route("customer-phones/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = phones.getItem(id);
			response.status = true;
			return response;
		}

	}
}
