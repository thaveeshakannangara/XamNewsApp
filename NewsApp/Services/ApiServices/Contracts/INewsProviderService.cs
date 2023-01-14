using NewsAPI.Models;
using System.Threading.Tasks;

namespace NewsApp.Services.ApiServices.Contracts
{
	public interface INewsProviderService
	{
		Task<ArticlesResult> GetTopNewsUpdates(TopHeadlinesRequest request);
	}
}