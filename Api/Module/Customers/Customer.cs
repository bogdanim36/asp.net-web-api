using System;
using System.Collections.Generic;
using WebApi.Api.Base;
using System.Data;

namespace WebApi.Api.Customers
{
	public class Customer
	{
		private DataAccess dataAccess;
		private String tableName = "customers";
		public Customer()
		{
			dataAccess = new DataAccess("MySQL");
		}
		public CustomerModel getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };
			DataTable data = dataAccess.ExecuteQuery(
				@"SELECT a.*, 
							c.name as city, c.nameInvariant as cityInvariant,
							b.name as country, b.nameInvariant as countryInvariant
						FROM  " + this.tableName + @" a 
						LEFT JOIN countries b on b.id = a.countryId
						LEFT JOIN cities c on c.id = a.cityId
						WHERE id =?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private CustomerModelExtended ConvertToItem(DataRow row)
		{
			return new CustomerModelExtended
			{
				id = Convert.ToInt32(row["id"]),
				name = Convert.ToString(row["name"]),
				nameInvariant = Convert.ToString(row["nameInvariant"]),
				code = Convert.ToString(row["code"]),
				administratorName = Convert.ToString(row["administratorName"]),
				seatAddress = Convert.ToString(row["seatAddress"]),
				cityId = Convert.ToInt32(row["cityId"]),
				city = Convert.ToString(row["city"]),
				cityInvariant = Convert.ToString(row["cityInvariant"]),
				countryId = Convert.ToInt32(row["countryId"]),
				country = Convert.ToString(row["country"]),
				countryInvariant = Convert.ToString(row["countryInvariant"]),
			};
		}
		public IEnumerable<CustomerModelExtended> getItems()
		{
			DataTable data = dataAccess.ExecuteQuery(
				@"SELECT a.*, 
							c.name as city, c.nameInvariant as cityInvariant,
							b.name as country, b.nameInvariant as countryInvariant
						FROM  " + this.tableName + @" a 
						LEFT JOIN countries b on b.id = a.countryId
						LEFT JOIN cities c on c.id = a.cityId");
			return ConvertToItems(data);
		}
		private IEnumerable<CustomerModelExtended> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
	}
}