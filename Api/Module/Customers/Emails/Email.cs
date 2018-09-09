using System;
using System.Collections.Generic;
using WebApi.Api.Base;
using System.Data;

namespace WebApi.Api.Customers.Emails
{
	public class Email
	{
		public string tableName = "customers-emails";
		private DataAccess dataAccess;
		public Email()
		{
			dataAccess = new DataAccess("MySQL");
		}
		public EmailModel getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };

			DataTable data = dataAccess.ExecuteQuery("Select * from `"+this.tableName+"` where id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private EmailModel ConvertToItem(DataRow row)
		{
			return new EmailModel
			{
				id = Convert.ToInt32(row["id"]),
				customerId = Convert.ToInt32(row["customerId"]),
				name = Convert.ToString(row["name"]),
				email = Convert.ToString(row["email"]),
			};
		}
		public IEnumerable<EmailModel> getItems(int customerId)
		{
			ObjectVar par = new ObjectVar { Name = "?customerId", Value = customerId };
			DataTable data = dataAccess.ExecuteQuery("Select * from `" + this.tableName + "` where customerId=?", par);
			return ConvertToItems(data);
		}
		private IEnumerable<EmailModel> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
	}
}