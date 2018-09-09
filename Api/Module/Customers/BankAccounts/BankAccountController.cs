using System.Web.Http;
using WebApi.Api.Base;

namespace WebApi.Api.Customers.BankAccounts
{
	public class BankAccountController : ApiController
	{
		BankAccount bankAccount = new BankAccount();

		[HttpGet]
		[AllowAnonymous]
		[Route("customer-accounts/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = bankAccount.getItem(id);
			response.status = true;
			return response;
		}

	}
}
