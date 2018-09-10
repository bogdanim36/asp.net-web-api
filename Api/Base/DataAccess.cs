using System;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.Common;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace WebApi.Api.Base
{
	public class DataAccess
	{
		public String connectionName { get; set; }
		public String connectionString { get; set; }
		public String dbName { get; set; }
		public String server { get; set; }
		public String user { get; set; }
		public String password { get; set; }
		public String providerName { get; set; }
		public System.Data.Common.DbConnection con { get; set; }
		private System.Data.Common.DbProviderFactory provider;
		public DataSet dataSet { get; set; }
		public DbDataAdapter dataAdapter;
		public DataTable tableStruct { get; set; }
		public string tableName { get; set; }
		private string _identityColumn;
		public string identityColumn
		{
			get { return _identityColumn; }
			set { _identityColumn = value.ToLower(); }
		}

		public DataAccess() { }

		public DataAccess(string _connectionName)
		{
			connectionName = _connectionName;
		}
		public DataAccess(string _connectionName, string _tableName, string idField)
		{
			connectionName = _connectionName;
			tableName = _tableName;
			identityColumn = idField;
		}
		public string GetProviderFactoryLib(string type)
		{
			string MySql = ProviderFactoryType.MySql.ToString();
			if (type == ProviderFactoryType.MySql.ToString()) return "MySql.Data.MySqlClient";
			if (type == ProviderFactoryType.SQLServer.ToString()) return "System.Data.SqlClient";
			if (type == ProviderFactoryType.Oracle.ToString()) return "System.Data.OracleClient";
			if (type == ProviderFactoryType.MsJet.ToString()) return "System.Data.Oledb";
			return null;

		}
		public void readConectionSettings()
		{

			dbName = "axcompdemo";
			server = "localhost";
			user = "axcompdemo";
			password = "8iNbR7HgaxJGlJnq";
			providerName = "MySql";
		}
		public string BuildConnectionString(string providerName)
		{
			return BuildConnectionString(providerName, "");
		}
		public string BuildConnectionString(string providerName, string dataBase)
		{
			String connString = null;
			if (providerName == ProviderFactoryType.MySql.ToString())
			{
				String connOptions = Convert.ToString(1 + 16 + 32 + 64 + 128 + 2048 + 16384 + 2097152 + 1048576 + 4194304 + 67108864 + 268435456);
				if (dataBase == "")
				{ connString = String.Concat("Server=", this.server, ";Uid=", this.user, ";Pwd=", this.password, "; convert zero datetime=True", ";"); }
				else
				{ connString = String.Concat("Server=", this.server, ";Database=", dataBase, ";Uid=", this.user, ";Pwd=", this.password, "; convert zero datetime=True", ";"); }
				return connString;
			}
			if (providerName == ProviderFactoryType.SQLServer.ToString()) return "System.Data.SqlClient";
			if (providerName == ProviderFactoryType.Oracle.ToString()) return "System.Data.OracleClient";
			if (providerName == ProviderFactoryType.MsJet.ToString()) return "System.Data.Oledb";
			return connString;
		}

		public bool LoadConnection()
		{

			if (connectionName == null)
			{
				return false;
			}
			if (providerName == null)
			{
				this.readConectionSettings();
			}
			string lib = GetProviderFactoryLib(providerName);
			provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, this.dbName);
			this.con = con;
			return true;
		}
		private System.Data.Common.DbProviderFactory RegisterProvider(string providerName)
		{
			System.Data.Common.DbProviderFactory f = null;
			if (this.providerName == ProviderFactoryType.MySql.ToString())
			{
				using (var dt = new DataTable())
				{
					dt.Columns.Add("Name");
					dt.Columns.Add("Description");
					dt.Columns.Add("InvariantName");
					dt.Columns.Add("AssemblyQualifiedName");
					dt.Rows.Add("ADO.Net driver for MySQL",
						 "mysql more",
						 "mysqlClient",
						 "MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data, Version=6.7.4.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d");
					f = DbProviderFactories.GetFactory(dt.Rows[0]);
				}
			}
			if (this.providerName == ProviderFactoryType.SQLServer.ToString())
			{
				using (var dt = new DataTable())
				{
					dt.Columns.Add("Name");
					dt.Columns.Add("Description");
					dt.Columns.Add("InvariantName");
					dt.Columns.Add("AssemblyQualifiedName");
					dt.Rows.Add("SqlClient Data Provider",
						 "SqlClient Data Provider",
						 "System.Data.SqlClient",
						 "System.Data.SqlClient.SqlClientFactory, System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
					f = DbProviderFactories.GetFactory(dt.Rows[0]);
				}
			}
			return f;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public DataTable getDataTable(String queryString, params dynamic[] args)
		{
			DataTable table = null;
			queryString = queryString.Replace("\r", " ").Replace("\n", "").Replace("\t", " ");
			Console.WriteLine("Select cmd: " + queryString);
			string lib = GetProviderFactoryLib(providerName);
			System.Data.Common.DbProviderFactory provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, this.dbName);
			dataSet = new DataSet();
			if (dataAdapter == null)
			{
				if (this.tableName == null) SetTableName(queryString);

				tableStruct = RetrieveTableStructure(this.tableName);
				if (tableStruct.Columns.Count == 0)
					return null;
				dataAdapter = provider.CreateDataAdapter();
				dataAdapter.SelectCommand = BuildSelectCommand(provider, con, queryString, args); ;
				dataAdapter.DeleteCommand = BuildDeleteCommand(provider, con);
				dataAdapter.UpdateCommand = BuildUpdateCommand(provider, con);
				dataAdapter.InsertCommand = BuildInsertCommand(provider, con);
				dataAdapter.AcceptChangesDuringFill = true;
				dataAdapter.AcceptChangesDuringUpdate = true;
				dataAdapter.ContinueUpdateOnError = false;

			}

			if (con.ConnectionString != dataAdapter.SelectCommand.Connection.ConnectionString)
			{
				dataAdapter.SelectCommand = BuildSelectCommand(provider, con, queryString, args); ;
				dataAdapter.DeleteCommand = BuildDeleteCommand(provider, con);
				dataAdapter.UpdateCommand = BuildUpdateCommand(provider, con);
				dataAdapter.InsertCommand = BuildInsertCommand(provider, con);

			}
			if (args.Length > 0)
			{
				dataAdapter.SelectCommand.Parameters.Clear();
				foreach (var param in args)
				{
					DbParameter par = provider.CreateParameter();
					par.Direction = ParameterDirection.Input;
					par.ParameterName = param.Name;
					par.Value = param.Value;
					dataAdapter.SelectCommand.Parameters.Add(par);
				}
			}
			try
			{
				con.Open();
				//BuildTableMapping(dataAdapter);
				dataAdapter.Fill(dataSet, this.tableName);
				table = dataSet.Tables[this.tableName];
				dataAdapter.MissingMappingAction = MissingMappingAction.Passthrough;
				if (identityColumn != null)
				{
					if (table.Columns.Contains(identityColumn))
					{
						table.PrimaryKey = new DataColumn[] { table.Columns[identityColumn] };
						table.Columns[identityColumn].AllowDBNull = true;
					}
					else
					{
						throw new Exception(String.Format("Tabela {0}, nu contine coloana Identity ={1}", tableName, identityColumn));
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				con.Close();
			}
			return table;
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public DataTable ExecuteQuery(String queryString, params dynamic[] args)
		{

			readConectionSettings();
			string lib = GetProviderFactoryLib(providerName);
			provider = RegisterProvider(providerName);
			con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, this.dbName);
			DataTable table = null;
			DbCommand com = provider.CreateCommand();
			com.CommandTimeout = 30;
			com.Connection = con;
			com.CommandText = queryString;
			com.CommandType = CommandType.Text;
			dataSet = new DataSet();
			dataAdapter = provider.CreateDataAdapter();
			dataAdapter.SelectCommand = com;
			foreach (var param in args)
			{
				DbParameter par = provider.CreateParameter();
				par.Direction = ParameterDirection.Input;
				par.ParameterName = param.Name;
				par.Value = param.Value;
				dataAdapter.SelectCommand.Parameters.Add(par);
			}

			try
			{
				con.Open();
				dataAdapter.Fill(dataSet);
				table = dataSet.Tables[0];
			}
			catch (Exception ex)
			{
				Console.Write(ex);
				throw ex;
			}
			con.Close();
			return table;
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public DataTable getDataQuery(String queryString, params dynamic[] args)
		{
			if (con == null) LoadConnection();
			DataTable table = null;
			DbCommand com = provider.CreateCommand();
			com.CommandTimeout = 30;
			com.Connection = this.con;
			com.CommandText = queryString;
			com.CommandType = CommandType.Text;
			dataSet = new DataSet();
			dataAdapter = provider.CreateDataAdapter();
			dataAdapter.SelectCommand = com;
			if (args != null && args.Length > 0)
			{
				foreach (var param in args)
				{
					DbParameter par = provider.CreateParameter();
					par.Direction = ParameterDirection.Input;
					par.ParameterName = param.Name;
					par.Value = param.Value;
					dataAdapter.SelectCommand.Parameters.Add(par);
				}
			}
			try
			{
				con.Open();
				dataAdapter.Fill(dataSet);
				table = dataSet.Tables[0];
			}
			catch (Exception ex)
			{
				throw ex;
			}
			con.Close();
			return table;
		}
		private String LastInsertIdCommand(string providerName)
		{
			string comText = "";
			if (providerName.Equals(ProviderFactoryType.MySql.ToString()))
			{ return "SELECT LAST_INSERT_ID();"; }
			if (providerName.Equals(ProviderFactoryType.SQLServer.ToString()))
			{ return "SELECT SCOPE_IDENTITY();"; }
			if (providerName.Equals(ProviderFactoryType.Oracle.ToString()))
			{ return "SELECT SEQNAME.CURRVAL FROM DUAL;"; }
			if (providerName.Equals(ProviderFactoryType.PostgreSQL.ToString()))
			{ return "SELECT lastval();"; }

			return comText;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private int LastInsertedId()
		{
			string lib = GetProviderFactoryLib(providerName);
			System.Data.Common.DbProviderFactory provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, this.dbName);
			DbCommand com = provider.CreateCommand();
			com.CommandTimeout = 30;
			com.Connection = con;
			com.CommandText = LastInsertIdCommand(providerName);
			com.CommandType = CommandType.Text;
			int lastId = 0;
			try
			{
				con.Open();
				com.Prepare();
				lastId = Convert.ToInt32(com.ExecuteScalar());
			}
			catch (Exception ex)
			{
				throw ex;
			}
			con.Close();
			return lastId;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private DbCommand BuildSelectCommand(System.Data.Common.DbProviderFactory provider, DbConnection con, string queryString, params dynamic[] args)
		{
			DbCommand com = provider.CreateCommand();
			com.CommandTimeout = 30;
			com.Connection = con;
			com.CommandText = queryString;
			com.CommandType = CommandType.Text;
			foreach (var param in args)
			{
				DbParameter par = provider.CreateParameter();
				par.Direction = ParameterDirection.Input;
				par.ParameterName = param.Name;
				par.Value = param.Value;
				com.Parameters.Add(par);
			}
			return com;
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private DbCommand BuildUpdateCommand(System.Data.Common.DbProviderFactory provider, DbConnection con)
		{
			DbCommand com = provider.CreateCommand();
			com.CommandType = CommandType.Text;
			com.Connection = con;
			string fieldList = SetFieldList();
			if (providerName == ProviderFactoryType.MySql.ToString())
			{
				com.CommandText = String.Format("Update {0} SET {2} WHERE {1} = ?{1}", this.tableName, this.identityColumn, fieldList);
				foreach (DataRow row in this.tableStruct.Rows)
				{
					MySqlParameter parameter = new MySqlParameter();
					parameter.ParameterName = "?" + row["fieldName"];
					parameter.MySqlDbType = GetMySqlType(row["ValueType"].ToString());
					parameter.Direction = ParameterDirection.Input;
					parameter.SourceColumn = row["fieldCaption"].ToString();
					com.Parameters.Add(parameter);
				}
			}
			return com;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private DbCommand BuildInsertCommand(System.Data.Common.DbProviderFactory provider, DbConnection con)
		{
			DbCommand com = provider.CreateCommand();
			com.CommandType = CommandType.Text;
			com.Connection = con;
			string fieldList = SetFieldList();
			if (providerName == ProviderFactoryType.MySql.ToString())
			{
				com.CommandText = String.Format("Insert Into {0} SET {1}", this.tableName, fieldList) + ";" + this.LastInsertIdCommand(providerName);
				foreach (DataRow row in this.tableStruct.Rows)
				{
					MySqlParameter parameter = new MySqlParameter();
					parameter.ParameterName = "?" + row["fieldName"];
					parameter.MySqlDbType = GetMySqlType(row["ValueType"].ToString());
					parameter.SourceColumn = row["fieldName"].ToString();
					parameter.IsNullable = true;
					if (row["fieldName"].ToString() == this.identityColumn)
					{ parameter.Direction = ParameterDirection.Output; }
					else
					{ parameter.Direction = ParameterDirection.Input; }
					com.Parameters.Add(parameter);
				}
			}
			return com;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		private DbCommand BuildDeleteCommand(System.Data.Common.DbProviderFactory provider, DbConnection con)
		{
			DbCommand com = provider.CreateCommand();
			com.CommandType = CommandType.Text;
			com.Connection = con;
			if (providerName == ProviderFactoryType.MySql.ToString())
			{
				com.CommandText = String.Format("Delete From {0} WHERE {1} = ?{1}", this.tableName, this.identityColumn);

				MySqlParameter parameter = new MySqlParameter();
				parameter.ParameterName = "?" + this.identityColumn;
				parameter.MySqlDbType = MySqlDbType.Int32;
				parameter.Direction = ParameterDirection.Input;
				parameter.SourceColumn = this.identityColumn;
				com.Parameters.Add(parameter);

			}
			return com;
		}

		private DataTableMapping BuildTableMapping(DataAdapter adapter)
		{
			DataTableMapping mapping = adapter.TableMappings.Add(tableName, tableName);

			foreach (DataRow row in this.tableStruct.Rows)
			{
				string colName = row["fieldName"].ToString();
				string colCaption = row["fieldCaption"].ToString();
				mapping.ColumnMappings.Add(colName, colCaption);

			}
			return mapping;
		}

		private string SetFieldList()
		{
			string str = "";
			foreach (DataRow row in this.tableStruct.Rows)
			{

				string colName = row["FieldCaption"].ToString();
				if (colName.Equals(this.identityColumn)) continue;

				if (str != "")
				{ str += ","; }
				str += colName + "=?" + colName;

			}
			return str;
		}

		private MySqlDbType GetMySqlType(string typeName)
		{
			switch (typeName.ToLower())
			{
				case "date":
					return MySqlDbType.Date;
				case "datetime":
					return MySqlDbType.DateTime;
				case "text":
					return MySqlDbType.Text;
				case "varchar":
				case "char":
					return MySqlDbType.VarChar;
				case "tinyint":
					return MySqlDbType.UInt16;
				case "decimal":
					return MySqlDbType.Decimal;
				case "mediumint":
					return MySqlDbType.Int32;
				case "int":
					return MySqlDbType.Int32;
				default:
					throw new Exception("Completeaza GetMyySqlType pt. " + typeName);
			}
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public DataTable RetrieveTableStructure(string tableName)
		{
			DataTable tableStruct = new DataTable();
			string lib = GetProviderFactoryLib(providerName);
			System.Data.Common.DbProviderFactory provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, "");
			DbCommand com = provider.CreateCommand();
			com.CommandTimeout = 30;
			com.Connection = con;
			string queryString = "";
			if (providerName == ProviderFactoryType.MySql.ToString())
			{
				queryString = string.Format(@" 
					SELECT 
						lower( Column_name) as FieldName, 
						Column_name as FieldCaption, 
						Data_type As ValueType, 
						0 As ColumnIsHide, 
						0 As FieldIsHide, 
						0 As IsFake, 
						0 As IsReadOnly, 
						0 As IsRequiered, 
						'' As FieldCategory,
						'' As UserHelp,
						Extra As Extra, 
						if( isnull( Character_Maximum_Length),Numeric_Precision, Character_Maximum_Length) As Length, 
						if( isnull(Numeric_Scale), 000, Numeric_Scale) As DecimalPosition 
					From information_schema.COLUMNS
					Where Table_Schema = '{0}'
						And LOWER(Table_Name) = '{1}'", this.dbName, tableName.ToLower()).Replace("\r", "").Replace("\n", " ").Replace("\t", " ").Replace("  ", " ");

			}
			com.CommandText = queryString;
			com.CommandType = CommandType.Text;
			DataSet ds = new DataSet();
			DbDataAdapter da = provider.CreateDataAdapter();
			da.SelectCommand = com;

			try
			{
				con.Open();
				da.Fill(ds);
				tableStruct = ds.Tables[0];
			}
			catch (Exception ex)
			{
				throw ex;

			}
			con.Close();

			return tableStruct;
		}

		public void SetTableName(string selectString)
		{
			selectString = selectString.Replace("\r", " ").Replace("\t", " ").Replace("\n", "");
			int first = selectString.ToUpper().IndexOf("FROM ") + 5;
			string tmpStr = selectString.Substring(first);
			int next = tmpStr.IndexOf(" ");
			if (next == -1)
			{ next = tmpStr.Length; }
			this.tableName = tmpStr.Substring(0, next);
		}

		public void RecordInsert(out long lastInsertId, DataRow newRow)
		{
			lastInsertId = 0;
			Console.WriteLine("DataAccesClass.RecordInsert ");
			DbCommand addCmd = dataAdapter.InsertCommand;
			Console.WriteLine("Insert cmd: " + addCmd.CommandText);
			try
			{
				addCmd.Connection.Open();
				foreach (DbParameter parameter in addCmd.Parameters)
				{
					string fieldName = parameter.SourceColumn;
					if (fieldName == this.identityColumn) continue;
					parameter.Value = newRow[fieldName];
				}
				object returnVal = addCmd.ExecuteScalar();
				lastInsertId = Convert.ToInt64(returnVal);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{ addCmd.Connection.Close(); }

			Console.Write(" lastinsertId={0}", lastInsertId);
			Console.WriteLine("");
		}
		public void RecordDelete(out int rowsAffected, object recordId)
		{
			rowsAffected = 0;
			Console.WriteLine("DataAccesClass.RecordDelete Id={0}", recordId);
			DbCommand delCmd = dataAdapter.DeleteCommand;
			Console.WriteLine("Delete cmd: " + delCmd.CommandText);
			try
			{
				delCmd.Connection.Open();
				DbParameter parameter = delCmd.Parameters[0];
				parameter.Value = recordId;
				parameter.SourceVersion = DataRowVersion.Original;

				object returnVal = delCmd.ExecuteNonQuery();
				rowsAffected = Convert.ToInt16(returnVal);
			}
			catch (Exception ex)
			{ throw ex; }
			finally
			{ delCmd.Connection.Close(); }

			Console.Write(" rowAffected={0}", rowsAffected);
			Console.WriteLine("");
		}
		public void RecordUpdate(out int rowsAffected, DataRow row)
		{
			rowsAffected = 0;
			readConectionSettings();
			string lib = GetProviderFactoryLib(providerName);
			provider = RegisterProvider(providerName);
			con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, dbName);

			tableStruct = RetrieveTableStructure(tableName);
			if (tableStruct.Columns.Count == 0) return;
			dataAdapter = provider.CreateDataAdapter();
			dataAdapter.UpdateCommand = BuildUpdateCommand(provider, con);
			dataAdapter.AcceptChangesDuringFill = true;
			dataAdapter.AcceptChangesDuringUpdate = true;
			dataAdapter.ContinueUpdateOnError = false;

			DbCommand updCmd = dataAdapter.UpdateCommand;
			Console.WriteLine("Update cmd: " + updCmd.CommandText);
			try
			{
				updCmd.Connection.Open();

				foreach (DbParameter parameter in updCmd.Parameters)
				{
					string fieldName = parameter.SourceColumn;
					parameter.Value = row[fieldName];
					if (fieldName == identityColumn) parameter.SourceVersion = DataRowVersion.Original;
				}
				object returnVal = updCmd.ExecuteNonQuery();
				rowsAffected = Convert.ToInt16(returnVal);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{ updCmd.Connection.Close(); }


		}
		public void RecordUpdate<T>(out int rowsAffected, T row)
		{
			rowsAffected = 0;
			readConectionSettings();
			string lib = GetProviderFactoryLib(providerName);
			provider = RegisterProvider(providerName);
			con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, dbName);

			tableStruct = RetrieveTableStructure(tableName);
			if (tableStruct.Columns.Count == 0) return;
			dataAdapter = provider.CreateDataAdapter();
			dataAdapter.UpdateCommand = BuildUpdateCommand(provider, con);
			dataAdapter.AcceptChangesDuringFill = true;
			dataAdapter.AcceptChangesDuringUpdate = true;
			dataAdapter.ContinueUpdateOnError = false;

			DbCommand updCmd = dataAdapter.UpdateCommand;
			Console.WriteLine("Update cmd: " + updCmd.CommandText);
			try
			{
				updCmd.Connection.Open();

				foreach (DbParameter parameter in updCmd.Parameters)
				{
					string fieldName = parameter.SourceColumn;
					parameter.Value = row.GetType().GetProperty(fieldName+'1').GetValue(row, null);
					if (fieldName == identityColumn) parameter.SourceVersion = DataRowVersion.Original;
				}
				object returnVal = updCmd.ExecuteNonQuery();
				rowsAffected = Convert.ToInt16(returnVal);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{ updCmd.Connection.Close(); }


		}
		private string DataSetError(DataSet ds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Exista erori la salvarea datelor in fisierul: ");
			foreach (DataTable table in dataSet.Tables)
			{
				if (table.HasErrors)
				{
					stringBuilder.Append(table.TableName);
					foreach (DataRow row in table.Rows)
					{
						if (row.HasErrors)
						{
							stringBuilder.AppendLine("Eroare: " + row.RowError);
							stringBuilder.AppendLine("Inregistrare: " + row[this.identityColumn]);
							foreach (DataColumn column in table.Columns)
							{
								stringBuilder.AppendFormat("\r\n{0} = {1}", column.ColumnName, row[column]);
							}
						}
					}
				}
			}
			return stringBuilder.ToString();

		}
		public void RecordSave(out int rowsAffected)
		{
			rowsAffected = 0;
			Console.WriteLine("DataAccesClass.RecorSave ");
			try
			{
				rowsAffected = this.dataAdapter.Update(this.dataSet.Tables[0]);
			}
			catch (Exception ex)
			{ throw ex; }

		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public int ExecuteNonQuery(String queryString, bool silence, params dynamic[] args)
		{
			string lib = GetProviderFactoryLib(providerName);
			System.Data.Common.DbProviderFactory provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, this.dbName);
			DbCommand com = provider.CreateCommand();
			com.CommandTimeout = 30;
			com.Connection = con;
			com.CommandText = queryString;
			com.CommandType = CommandType.Text;
			foreach (var param in args)
			{
				DbParameter par = provider.CreateParameter();
				par.Direction = ParameterDirection.Input;
				par.ParameterName = param.Name;
				par.Value = param.Value;
				com.Parameters.Add(par);
			}

			int rowsAffected = 0;
			try
			{
				con.Open();
				object result = com.ExecuteNonQuery();
				if (result != null)
				{
					rowsAffected = Convert.ToInt32(result);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			con.Close();
			return rowsAffected;
		}

		public int ExecuteNonQuery(String queryString, params dynamic[] args)
		{
			return ExecuteNonQuery(queryString, false, args);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public int ExecuteNonQuery(String queryString, out int rowsAffected)
		{
			string lib = GetProviderFactoryLib(providerName);
			System.Data.Common.DbProviderFactory provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, this.dbName);
			rowsAffected = 0;
			try
			{
				con.Open();
				DbCommand com = provider.CreateCommand();
				com.CommandTimeout = 30;
				com.Connection = con;
				com.CommandText = queryString;
				com.CommandType = CommandType.Text;
				object result = com.ExecuteNonQuery();
				if (result != null)
				{
					rowsAffected = Convert.ToInt32(result);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			con.Close();
			return 0;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public int CreateDatabase(String queryString)
		{
			string lib = GetProviderFactoryLib(providerName);
			System.Data.Common.DbProviderFactory provider = RegisterProvider(providerName);
			DbConnection con = provider.CreateConnection();
			con.ConnectionString = BuildConnectionString(providerName, "");
			int rowsAffected = 0;
			try
			{
				con.Open();
				DbCommand com = provider.CreateCommand();
				com.CommandTimeout = 30;
				com.Connection = con;
				com.CommandText = queryString;
				com.CommandType = CommandType.Text;
				object result = com.ExecuteNonQuery();
				if (result != null)
				{
					rowsAffected = Convert.ToInt32(result);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			con.Close();
			return rowsAffected;
		}

		private MySqlParameter AddMySqlParameter(string columnName, string valueType)
		{
			MySqlParameter parameter = new MySqlParameter();
			parameter.ParameterName = "?" + columnName;
			parameter.MySqlDbType = GetMySqlType(valueType);
			parameter.SourceColumn = columnName;
			parameter.IsNullable = true;
			parameter.Direction = ParameterDirection.Input;
			return parameter;
		}
	}

}


