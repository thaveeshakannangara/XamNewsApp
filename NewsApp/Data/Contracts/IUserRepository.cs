using NewsApp.Models.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsApp.Data.Contracts
{
	public interface IUserRepository
	{
		Task<bool> SaveUserAync(UserDataModel words);

		Task<List<UserDataModel>> GetAllUsersAync();

		Task<UserDataModel> GetUserAync(string email, string password);
	}
}