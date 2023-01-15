using NewsAPI.Constants;
using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using System.Collections.Generic;

namespace NewsApp.Services.FilterService
{
	public class FilterOptionsService : IFilterOptionsService
	{
		public List<FilterModel> GetFilterOptions()
		{
			List<FilterModel> filterList = new List<FilterModel>();

			filterList.Add(new FilterModel()
			{
				FilterOption = Categories.Health.ToString(),
				IsSelected = true
			});

			filterList.Add(new FilterModel()
			{
				FilterOption = Categories.Business.ToString(),
				IsSelected = false
			});

			filterList.Add(new FilterModel()
			{
				FilterOption = Categories.Technology.ToString(),
				IsSelected = false
			});

			filterList.Add(new FilterModel()
			{
				FilterOption = Categories.Science.ToString(),
				IsSelected = false
			});

			filterList.Add(new FilterModel()
			{
				FilterOption = Categories.Sports.ToString(),
				IsSelected = false
			});

			filterList.Add(new FilterModel()
			{
				FilterOption = Categories.Entertainment.ToString(),
				IsSelected = false
			});
			return filterList;
		}

		public Categories GetFilterOptionsEnum(string category)
		{
			switch (category)
			{
				case "Health":
					return Categories.Health;

				case "Business":
					return Categories.Business;

				case "Science":
					return Categories.Science;

				case "Sports":
					return Categories.Sports;

				case "Technology":
					return Categories.Technology;

				case "Entertainment":
					return Categories.Entertainment;

				default:
					return Categories.Health;
			}
		}
	}
}