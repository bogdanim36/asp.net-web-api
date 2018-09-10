namespace WebApi.Api.Customers
{
	public class CustomerModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public string nameInvariant { get; set; }
		public string code { get; set; }
		public string administratorName { get; set; }
		public int countryId { get; set; }
		public int cityId { get; set; }
		public string seatAddress { get; set; }
	}
}