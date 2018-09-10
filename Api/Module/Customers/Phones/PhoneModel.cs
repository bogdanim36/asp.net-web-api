namespace WebApi.Api.Customers.Phones
{
	public class PhoneModel
	{
		public int id { get; set; }
		public int customerId { get; set; }
		public string name { get; set; }
		public string phone { get; set; }
	}
}