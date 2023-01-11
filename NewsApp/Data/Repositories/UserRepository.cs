using NewsApp.Data.Contracts;
using NewsApp.Models.DataModels;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsApp.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly SQLiteAsyncConnection _connection;

		public UserRepository(ISqliteConnectionManager _SQLiteconnection)
		{
			_connection = _SQLiteconnection.GetConnection();
		}

		public async Task<bool> SaveUserAync(UserDataModel words)
		{
			//Checking user already exists
			var user = await _connection.Table<UserDataModel>().FirstOrDefaultAsync(x => x.Email.ToLower() == words.Email.ToLower());

			if (user == null)
			{
				var result = await _connection.InsertAsync(words);
				if (result >= 1) return true;
			}
			return false;
		}

		public async Task<List<UserDataModel>> GetAllUsersAync()
		{
			return await _connection.Table<UserDataModel>().ToListAsync();
		}

		public async Task<UserDataModel> GetUserAync(string email, string password)
		{
			return await _connection.Table<UserDataModel>().FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
		}
	}
}