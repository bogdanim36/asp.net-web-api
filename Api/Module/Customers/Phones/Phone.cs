using System;
using System.Collections.Generic;
using WebApi.Api.Base;
using System.Data;

namespace WebApi.Api.Customers.Phones
{
	public class Phone
	{
		public string tableName = "customers-phones";
		private DataAccess dataAccess;
		public Phone()
		{
			dataAccess = new DataAccess("MySQL");
		}
		public PhoneModel getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };

			DataTable data = dataAccess.ExecuteQuery("Select * from `"+this.tableName+"` where id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private PhoneModel ConvertToItem(DataRow row)
		{
			return new PhoneModel
			{
				id = Convert.ToInt32(row["id"]),
				customerId = Convert.ToInt32(row["customerId"]),
				name = Convert.ToString(row["name"]),
				phone = Convert.ToString(row["phone"]),
			};
		}
		public IEnumerable<PhoneModel> getItems(int customerId)
		{
			ObjectVar par = new ObjectVar { Name = "?customerId", Value = customerId };
			DataTable data = dataAccess.ExecuteQuery("Select * from `" + this.tableName + "` where customerId=?", par);
			return ConvertToItems(data);
		}
		private IEnumerable<PhoneModel> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
	}
}