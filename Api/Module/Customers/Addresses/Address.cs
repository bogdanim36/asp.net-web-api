using System;
using System.Collections.Generic;
using WebApi.Api.Base;
using System.Data;

namespace WebApi.Api.Customers.Addresses
{
	public class Address
	{
		public string tableName = "customers-addresses";
		private DataAccess dataAccess;
		public Address()
		{
			dataAccess = new DataAccess("MySQL");
		}
		public AddressModelExtended getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };
			DataTable data = dataAccess.ExecuteQuery(
				@"Select a.*, 
							b.name as customer, b.nameInvariant as customerInvariant,
							c.name as country, c.nameInvariant as countryInvariant,
							d.name as city, d.nameInvariant as cityInvariant 
					From `" + this.tableName + @"` a 
					LEFT JOIN customers b on b.id = a.customerId
               LEFT JOIN countries c on c.id = a.countryId
               LEFT JOIN cities d on d.id = a.cityId
					Where a.id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private AddressModelExtended ConvertToItem(DataRow row)
		{
			return new AddressModelExtended
			{
				id = Convert.ToInt32(row["id"]),
				customerId = Convert.ToInt32(row["customerId"]),
				countryId = Convert.ToInt32(row["countryId"]),
				cityId = Convert.ToInt32(row["cityId"]),
				name = Convert.ToString(row["name"]),
				address = Convert.ToString(row["address"]),
				customer = Convert.ToString(row["customer"]),
				customerInvariant = Convert.ToString(row["customerInvariant"]),
				country = Convert.ToString(row["country"]),
				countryInvariant = Convert.ToString(row["countryInvariant"]),
				city = Convert.ToString(row["city"]),
				cityInvariant = Convert.ToString(row["cityInvariant"]),
			};
		}
		public IEnumerable<AddressModelExtended> getItems(int customerId)
		{
			ObjectVar par = new ObjectVar { Name = "?customerId", Value = customerId };
			DataTable data = dataAccess.ExecuteQuery(
				@"Select a.*, 
							b.name as customer, b.nameInvariant as customerInvariant,
							c.name as country, c.nameInvariant as countryInvariant,
							d.name as city, d.nameInvariant as cityInvariant 
					From `" + this.tableName + @"` a
					LEFT JOIN customers b on b.id = a.customerId
               LEFT JOIN countries c on c.id = a.countryId
               LEFT JOIN cities d on d.id = a.cityId
					Where customerId=?", par);
			return ConvertToItems(data);
		}
		private IEnumerable<AddressModelExtended> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
	}
}