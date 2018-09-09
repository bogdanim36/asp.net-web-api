using System.Web.Http;
using WebApi.Api.Base;
using WebApi.Api.Cities;
using WebApi.Api.Countries;
using WebApi.Api.Customers.Emails;
using WebApi.Api.Customers.Phones;
using WebApi.Api.Customers.BankAccounts;
using WebApi.Api.Customers.Addresses;

namespace WebApi.Api.Customers
{
	public class CustomersController : ApiController
	{
		City city = new City();
		Country country = new Country();
		Customer customer = new Customer();
		Phone phone = new Phone();
		Email email = new Email();
		BankAccount bankAccount = new BankAccount();
		Address address = new Address();

		[HttpGet]
		[AllowAnonymous]
		[Route("customers")]
		public ActionResponse Get()
		{
			var response = new GetCustomersResponse();
			response.data = customer.getItems();
			response.cities = city.getItems();
			response.countries = country.getItems();
			response.status = true;
			return response;
		}
		[HttpGet]
		[AllowAnonymous]
		[Route("customers/{id}")]
		public ActionResponse Get(int id)
		{
			var response = new ActionResponse();
			response.data = customer.getItem(id);
			response.status = true;
			return response;
		}
		[HttpPost]
		[AllowAnonymous]
		[Route("customers/getItemDetails")]
		public ActionResponse GetCustomerDetails([FromBody] CustomerModelExtended item)
		{
			var response = new ActionResponse();
			response.data = new GetCustomersDetails();
			response.data.phones = phone.getItems(item.id);
			response.data.emails = email.getItems(item.id);
			response.data.accounts = bankAccount.getItems(item.id);
			response.data.addresses = address.getItems(item.id);
			response.status = true;
			return response;
		}

	}
}
