using System.Data.Common;

namespace WebApi.Api.Base
{
	public abstract class DbProviderFactory
	{
		public abstract DbConnection CreateConnection();
		public abstract DbCommand CreateCommand();
		public abstract DbDataAdapter CreateDataAdapter();
	}
}