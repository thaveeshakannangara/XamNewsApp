using NewsAPI.Constants;
using NewsApp.Models.AppModels;
using System.Collections.Generic;

namespace NewsApp.Services.Contracts
{
	public interface ISortingService
	{
		List<SortingModel> GetSortingOptions();
		SortBys GetSortingOptionsEnum(string sortingOption);
	}
}