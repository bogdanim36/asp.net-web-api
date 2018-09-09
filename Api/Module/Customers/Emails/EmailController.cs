using System.Web.Http;
using WebApi.Api.Base;

namespace WebApi.Api.Customers.Emails

{
	public class EmailController : ApiController
	{
		Email email = new Email();

		[HttpGet]
		[AllowAnonymous]
		[Route("customer-emails/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = email.getItem(id);
			response.status = true;
			return response;
		}

	}
}
