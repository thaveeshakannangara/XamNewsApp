using NewsApp.Constants;
using SQLite;
using Xamarin.Essentials;
using DO = NewsApp.Models.DataModels;

namespace NewsApp.Data
{
	public class NewsSqliteDatabase
	{
		public NewsSqliteDatabase(string dbPath)
		{
			var options = new SQLiteConnectionString(dbPath,
				   AppContants.DbCreateFlags,
				   true, key: SecureStorage.GetAsync(PreferencesKey.DbKey).GetAwaiter().GetResult());
			SQLiteAsyncConnection _connection = new SQLiteAsyncConnection(options);

			_connection.CreateTableAsync<DO.UserDataModel>().GetAwaiter().GetResult();
		}
	}
}