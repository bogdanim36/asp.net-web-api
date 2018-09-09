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

			DataTable data = dataAccess.ExecuteQuery("Select * from Countries where id=?", par);
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
			DataTable data = dataAccess.ExecuteQuery("Select * from Countries");
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
			DataRow row = ConvertToDataRow(data);
			dataAccess.RecordUpdate(out rowAffected, row);
			var item = getItem(id);
			return item;
		}
		public DataRow ConvertToDataRow( CountryModel data)
		{
			DataTable dataTable = new DataTable();
			var src = data.GetType();
			foreach (PropertyInfo info in src.GetProperties())
			{
				dataTable.Columns.Add(info.Name);
			}
			dataTable.AcceptChanges();
			DataRow row = dataTable.NewRow();
			foreach (PropertyInfo info in src.GetProperties())
			{
				row[info.Name] = info.GetValue(data, null);
			}
			return row;
		}
	}
}