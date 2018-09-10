namespace WebApi.Api.Customers.Emails
{
	public class EmailModel
	{
		public int id { get; set; }
		public int customerId { get; set; }
		public string name { get; set; }
		public string email { get; set; }
	}
}