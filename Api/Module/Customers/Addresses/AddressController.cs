using System.Web.Http;
using WebApi.Api.Base;

namespace WebApi.Api.Customers.Addresses

{
	public class AddressController : ApiController
	{
		Address address = new Address();

		[HttpGet]
		[AllowAnonymous]
		[Route("customer-addresses/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = address.getItem(id);
			response.status = true;
			return response;
		}

	}
}
