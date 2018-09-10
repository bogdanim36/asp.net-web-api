namespace WebApi.Api.Customers.BankAccounts
{
	public class BankAccountModel
	{
		public int id { get; set; }
		public int customerId { get; set; }
		public string name { get; set; }
		public string account { get; set; }
		public string bank { get; set; }
	}
}