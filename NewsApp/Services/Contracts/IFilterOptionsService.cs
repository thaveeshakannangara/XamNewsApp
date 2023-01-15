using NewsAPI.Constants;
using NewsApp.Models.AppModels;
using System.Collections.Generic;

namespace NewsApp.Services.Contracts
{
	public interface IFilterOptionsService
	{
		List<FilterModel> GetFilterOptions();
		Categories GetFilterOptionsEnum(string category);
	}
}