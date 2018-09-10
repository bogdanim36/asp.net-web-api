namespace WebApi.Api.Customers
{
	public class CustomerModelExtended : CustomerModel
	{
		public string country { get; set; }
		public string countryInvariant { get; set; }
		public string city { get; set; }
		public string cityInvariant { get; set; }
	}
}