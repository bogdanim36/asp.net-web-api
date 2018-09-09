namespace WebApi.Api.Customers.Addresses
{
	public class AddressModel
	{
		public int id;
		public int customerId;
		public string name;
		public string address;
		public int countryId;
		public int cityId;
	}
}