using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsApp.Constants;
using NewsApp.Services.ApiServices.Contracts;
using System;
using System.Threading.Tasks;

namespace NewsApp.Services.ApiServices
{
	public class NewsProviderService : INewsProviderService
	{
		public async Task<ArticlesResult> GetTopNewsUpdates(TopHeadlinesRequest request)
		{
			try
			{
				var newsApiClient = new NewsApiClient(AppContants.NewsApiKey);
				var articlesResponse = await newsApiClient.GetTopHeadlinesAsync(new TopHeadlinesRequest
				{
					Category = request.Category,
					Language = request.Language,
					Page = request.Page,
					PageSize = request.PageSize,
				});

				if (articlesResponse.Status == Statuses.Ok)
				{
					return articlesResponse;
				}

				return new ArticlesResult();
			}
			catch (Exception ex)
			{
				return new ArticlesResult();
			}
		}
	}
}