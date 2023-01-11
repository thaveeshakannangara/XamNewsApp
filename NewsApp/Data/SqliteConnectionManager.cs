using NewsApp.Constants;
using NewsApp.Data.Contracts;
using SQLite;
using Xamarin.Essentials;

namespace NewsApp.Data
{
	public class SqliteConnectionManager : ISqliteConnectionManager
	{
		public SQLiteAsyncConnection GetConnection()
		{
			var options = new SQLiteConnectionString(Preferences.Get(PreferencesKey.DatabasePath, string.Empty),
			AppContants.SQLQueryFlags, true, key: SecureStorage.GetAsync(PreferencesKey.DbKey).GetAwaiter().GetResult());
			return new SQLiteAsyncConnection(options);
		}
	}
}