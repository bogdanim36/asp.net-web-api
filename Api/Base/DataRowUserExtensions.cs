using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApi.Api.Base
{
	///<summary>
	/// Extension methods for manipulating DataRows
	///</summary>
	public static class DataRowUserExtensions
	{
		/// <summary>
		/// Determines whether [is empty string] [the specified data row].
		/// </summary>
		/// <param name="dataRow">The data row.</param>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> if [is empty string] [the specified data row]; otherwise, including DBNull, <c>false</c>.
		/// </returns>
		public static bool IsEmptyString(this DataRow dataRow, string key)
		{
			if (dataRow.Table.Columns.Contains(key))
				return !dataRow.IsNull(key) && dataRow[key].ToString() == string.Empty;

			throw new ArgumentOutOfRangeException(key, dataRow, "does not contain column");
		}

		/// <summary>
		/// Determines whether the specified data row is null.
		/// </summary>
		/// <param name="dataRow">The data row.</param>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> if the specified data row is null; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNull(this DataRow dataRow, string key)
		{
			if (dataRow.Table.Columns.Contains(key))
				return dataRow[key] == null || dataRow[key] == DBNull.Value;

			throw new ArgumentOutOfRangeException(key, dataRow, "does not contain column");
		}

		/// <summary>
		/// Determines whether [is null or empty string] [the specified data row].
		/// </summary>
		/// <param name="dataRow">The data row.</param>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> if [is null or empty string] [the specified data row]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmptyString(this DataRow dataRow, string key)
		{
			if (dataRow.Table.Columns.Contains(key))
				return dataRow.IsNull(key) && dataRow.IsEmptyString(key);

			throw new ArgumentOutOfRangeException(key, dataRow, "does not contain column");
		}

		/// <summary>
		/// Gets the specified data row.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataRow">The data row.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static T Get<T>(this DataRow dataRow, string key)
		{
			if (dataRow.Table.Columns.Contains(key))
			{
				// If we're trying to convert an empty string to string return the string instead
				// default(string) which is null, otherwise return null if dataRow IsNullOrEmptyString
				// else convert
				return typeof(T) == typeof(string) && dataRow.IsEmptyString(key)
					 ? (T)dataRow[key] : (dataRow.IsNullOrEmptyString(key)
					 ? default(T) : (T)ChangeTypeTo<T>(dataRow[key]));
			}

			throw new ArgumentOutOfRangeException(key, dataRow, "does not contain column");
		}

		/// <summary>
		/// Changes the type to.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static object ChangeTypeTo<T>(this object value)
		{
			if (value == null)
				return null;

			Type underlyingType = typeof(T);
			if (underlyingType == null)
				throw new ArgumentNullException("value");

			if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				var converter = new NullableConverter(underlyingType);
				underlyingType = converter.UnderlyingType;
			}

			// Guid convert
			if (underlyingType == typeof(Guid))
			{
				return new Guid(value.ToString());
			}

			// Check for straight conversion or value.ToString conversion
			var objType = value.GetType();

			// If this is false, lets hope value.ToString can convert otherwise exception
			bool objTypeAssignable2typeT = underlyingType.IsAssignableFrom(objType);

			// Do conversion
			return objTypeAssignable2typeT ? Convert.ChangeType(value, underlyingType)
																			: Convert.ChangeType(value.ToString(), underlyingType);
		}
	}
}