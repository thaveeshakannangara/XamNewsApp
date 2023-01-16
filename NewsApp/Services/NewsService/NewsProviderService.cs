using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsApp.Constants;
using NewsApp.Services.Contracts;
using Plugin.FirebaseCrashlytics;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NewsApp.Services.NewsService
{
	public class NewsProviderService : INewsProviderService
	{
		public NewsProviderService()
		{
		}

		public async Task<ArticlesResult> GetTopNewsUpdates(TopHeadlinesRequest request)
		{
			var current = Connectivity.NetworkAccess;

			if (current == NetworkAccess.Internet)
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
					CrossFirebaseCrashlytics.Current.RecordException(ex);
					return new ArticlesResult();
				}
			}
			return new ArticlesResult();
		}

		public async Task<ArticlesResult> GetNewsAsync(EverythingRequest request)
		{
			var current = Connectivity.NetworkAccess;

			if (current == NetworkAccess.Internet)
			{
				try
				{
					var newsApiClient = new NewsApiClient(AppContants.NewsApiKey);
					var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
					{
						Q = request.Q,
						SortBy = request.SortBy,
						Language = request.Language,
						Page = request.Page,
						PageSize = request.PageSize,
						From = request.From,
						To = request.To
					});

					if (articlesResponse.Status == Statuses.Ok)
					{
						return articlesResponse;
					}

					return new ArticlesResult();
				}
				catch (Exception ex)
				{
					CrossFirebaseCrashlytics.Current.RecordException(ex);
					return new ArticlesResult();
				}
			}
			return new ArticlesResult();
		}
	}
}