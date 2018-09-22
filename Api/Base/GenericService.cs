using System;
using System.Collections.Generic;

using System.Data;

namespace WebApi.Api.Base
{
	public class GenericService <Model, ModelExtended>
	{
		private DataAccess dataAccess;
		private String tableName ;
		private String identityColumn;
		public GenericService()
		{
			dataAccess = new DataAccess("MySQL", tableName, identityColumn);
		}
		public ModelExtended getItem(int id)
		{
			ObjectVar par = new ObjectVar { Name = "?" + identityColumn, Value = id };
			string select = dataAccess.dataAdapter.SelectCommand.ToString();
			DataTable data = dataAccess.ExecuteQuery(
				@"SELECT a.*,
					 b.name as country, b.nameInvariant as countryInvariant
					FROM `" + this.tableName + @"` a
					LEFT JOIN `countries` b ON b.id = a.countryId 
					Where a.id=?", par);
			if (data.Rows.Count == 0) return null;
			else return ConvertToItem<ModelExtended>(data.Rows[0]);
		}
		private T ConvertToItem<T>(DataRow row) => new T(row);
		public IEnumerable<ModelExtended> getItems()
		{
			DataTable data = dataAccess.ExecuteQuery(
				@"SELECT a.*,
					 b.name as country, b.nameInvariant as countryInvariant
				  FROM `" + this.tableName + @"` a
				  LEFT JOIN `countries` b ON b.id = a.countryId");
			return ConvertToItems(data);
		}
		private IEnumerable<ModelExtended> ConvertToItems(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				yield return ConvertToItem(row);
			}
		}
	}
}