using NewsApp.ViewModels;
using System.Collections.Generic;

namespace NewsApp
{
	public class SearchListPageModel : BaseViewModel
	{

		private List<string> filterOptions;

		public List<string> FilterOptions
		{ 
			get => filterOptions;
			set
			{
				if (filterOptions == value) return;
				filterOptions = value;
				RaisePropertyChanged(nameof(FilterOptions));
			}
		}

		public SearchListPageModel()
		{
			filterOptions = new List<string>();
		}

		public override void Init(object initData)
		{
			//InitializeFilters
			FilterOptions = Constants.AppContants.FilterOptions;


			base.Init(initData);
		}

	}
}