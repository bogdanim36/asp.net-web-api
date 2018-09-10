namespace WebApi.Api.Customers.Addresses
{
	public class AddressModelExtended :AddressModel
	{
		public string customer { get; set; }
		public string customerInvariant { get; set; }
		public string country { get; set; }
		public string countryInvariant { get; set; }
		public string city { get; set; }
		public string cityInvariant { get; set; }
	}
}