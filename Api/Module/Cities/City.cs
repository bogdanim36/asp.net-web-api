using System;
using System.Collections.Generic;

using WebApi.Api.Base;
using System.Data;

namespace WebApi.Api.Cities
{
	public class City
	{
		private DataAccess dataAccess;
		private readonly String tableName = "cities";
		public City()
		{
			dataAccess = new DataAccess("MySQL");
		}
		public CityModel getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?id", Value = id };
			DataTable data = dataAccess.ExecuteQuery(
				@"SELECT a.*,
					 b.name as country, b.nameInvariant as countryInvariant
					FROM `" + this.tableName + @"` a
					LEFT JOIN `countries` b ON b.id = a.countryId 
					Where a.id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem(data.Rows[0]);
		}
		private CityModelExtended ConvertToItem(DataRow row)
		{
			return new CityModelExtended
			{
				id = Convert.ToInt32(row["id"]),
				countryId = Convert.ToInt32(row["countryId"]),
				name = Convert.ToString(row["name"]),
				nameInvariant = Convert.ToString(row["nameInvariant"]),
				country = Convert.ToString(row["country"]),
				countryInvariant = Convert.ToString(row["countryInvariant"]),
			};
		}
		public IEnumerable<CityModelExtended> getItems()
		{
			DataTable data = dataAccess.ExecuteQuery(
				@"SELECT a.*,
					 b.name as country, b.nameInvariant as countryInvariant
				  FROM `" + this.tableName + @"` a
				  LEFT JOIN `countries` b ON b.id = a.countryId");
			return ConvertToItems(data);
		}
		private IEnumerable<CityModelExtended> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}

		}
		public CityModel Update(int id, CityModel item)
		{
			return item;
		}
	}
}