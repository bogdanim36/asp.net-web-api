using System;
using System.Collections.Generic;
using WebApi.Api.Base;
using System.Data;

namespace WebApi.Api.Customers.BankAccounts
{
	public class BankAccount
	{
		public string tableName = "customers-accounts";
		private DataAccess dataAccess;
		public BankAccount()
		{
			dataAccess = new DataAccess("MySQL");
		}
		public BankAccountModel getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };

			DataTable data = dataAccess.ExecuteQuery("Select * from `"+this.tableName+"` where id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private BankAccountModel ConvertToItem(DataRow row)
		{
			return new BankAccountModel
			{
				id = Convert.ToInt32(row["id"]),
				customerId = Convert.ToInt32(row["customerId"]),
				name = Convert.ToString(row["name"]),
				account = Convert.ToString(row["account"]),
				bank = Convert.ToString(row["bank"]),
			};
		}
		public IEnumerable<BankAccountModel> getItems(int customerId)
		{
			ObjectVar par = new ObjectVar { Name = "?customerId", Value = customerId };
			DataTable data = dataAccess.ExecuteQuery("Select * from `" + this.tableName + "` where customerId=?", par);
			return ConvertToItems(data);
		}
		private IEnumerable<BankAccountModel> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
	}
}