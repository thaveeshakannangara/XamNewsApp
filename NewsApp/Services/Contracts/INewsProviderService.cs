using NewsAPI.Models;
using System.Threading.Tasks;

namespace NewsApp.Services.Contracts
{
	public interface INewsProviderService
	{
		Task<ArticlesResult> GetTopNewsUpdates(TopHeadlinesRequest request);

		Task<ArticlesResult> GetNewsAsync(EverythingRequest request);
	}
}