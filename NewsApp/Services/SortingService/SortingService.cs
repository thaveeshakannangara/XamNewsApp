using NewsAPI.Constants;
using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using System.Collections.Generic;

namespace NewsApp.Services.SortingService
{
	public class SortingService : ISortingService
	{
		public List<SortingModel> GetSortingOptions()
		{
			List<SortingModel> sortingList = new List<SortingModel>();

			sortingList.Add(new SortingModel()
			{
				SortingOption = SortBys.Popularity.ToString(),
				IsSelected = false
			});

			sortingList.Add(new SortingModel()
			{
				SortingOption = SortBys.PublishedAt.ToString(),
				IsSelected = false
			});

			sortingList.Add(new SortingModel()
			{
				SortingOption = SortBys.Relevancy.ToString(),
				IsSelected = false
			});

			return sortingList;
		}

		public SortBys GetSortingOptionsEnum(string sortingOption)
		{
			switch (sortingOption)
			{
				case "Relevancy":
					return SortBys.Relevancy;

				case "Popularity":
					return SortBys.Popularity;

				case "PublishedAt":
					return SortBys.PublishedAt;

				default:
					return SortBys.Relevancy;
			}
		}
	}
}