using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using NewsApp.ViewModels;
using System.Collections.Generic;

namespace NewsApp
{
	public class SearchListPageModel : BaseViewModel
	{
		private readonly IFilterOptionsService _filterService;
		private List<FilterModel> filterOptions;

		public List<FilterModel> FilterOptions
		{
			get => filterOptions;
			set
			{
				if (filterOptions == value) return;
				filterOptions = value;
				RaisePropertyChanged(nameof(FilterOptions));
			}
		}

		public SearchListPageModel(IFilterOptionsService filterService)
		{
			_filterService = filterService;
			filterOptions = new List<FilterModel>();
		}

		public override void Init(object initData)
		{
			//InitializeFilters
			FilterOptions = _filterService.GetFilterOptions();

			base.Init(initData);
		}
	}
}