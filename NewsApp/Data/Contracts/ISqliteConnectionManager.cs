using SQLite;

namespace NewsApp.Data.Contracts
{
	public interface ISqliteConnectionManager
	{
		SQLiteAsyncConnection GetConnection();
	}
}