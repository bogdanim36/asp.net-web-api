namespace WebApi.Api.Customers.Addresses
{
	public class AddressModel
	{
		public int id { get; set; }
		public int customerId { get; set; }
		public string name { get; set; }
		public string address { get; set; }
		public int countryId { get; set; }
		public int cityId { get; set; }
	}
}