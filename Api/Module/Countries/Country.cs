using System;
using System.Collections.Generic;
using WebApi.Api.Base;
using System.Data;
using System.Reflection;

namespace WebApi.Api.Countries
{
	public class Country
	{
		private DataAccess dataAccess;
		private string tableName = "countries";
		public Country()
		{
			dataAccess = new DataAccess("MySQL", tableName, "id");
		}
		public CountryModel getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };

			DataTable data = dataAccess.ExecuteQuery("Select * from " + tableName + " where id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private CountryModel ConvertToItem(DataRow row)
		{
			return new CountryModel
			{
				id = Convert.ToInt32(row["id"]),
				name = Convert.ToString(row["name"]),
				nameInvariant = Convert.ToString(row["nameInvariant"]),
			};
		}
		public IEnumerable<CountryModel> getItems()
		{
			DataTable data = dataAccess.ExecuteQuery("Select * from " + tableName);
			return ConvertToItems(data);
		}
		private IEnumerable<CountryModel> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
		public CountryModel Update(int id, CountryModel data)
		{
			var rowAffected = 0;
			dataAccess.RecordUpdate<CountryModel>(out rowAffected, data);
			var item = getItem(id);
			return item;
		}
	}
}