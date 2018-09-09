using System.Collections.Generic;
using WebApi.Api.Base;
using WebApi.Api.Cities;
using WebApi.Api.Countries;
using WebApi.Api.Customers.Addresses;
using WebApi.Api.Customers.BankAccounts;
using WebApi.Api.Customers.Emails;
using WebApi.Api.Customers.Phones;

namespace WebApi.Api.Customers
{
	public class GetCustomersDetails
	{
		public IEnumerable<PhoneModel> phones;
		public IEnumerable<EmailModel> emails;
		public IEnumerable<BankAccountModel> accounts;
		public IEnumerable<AddressModel> addresses;
	}
}