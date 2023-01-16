using AutoMapper;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsApp.Controls.BottomSheet;
using NewsApp.Helpers;
using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using NewsApp.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class SearchListPageModel : BaseViewModel
	{
		private readonly INewsProviderService _newsService;
		private readonly IFilterOptionsService _filterService;
		private List<FilterModel> filterOptions;
		private FilterModel selectedFilterOption;
		private NewsModel selectedNewsModel;
		private ObservableCollection<NewsModel> newsList;
		private string searchKey = string.Empty;
		private int totalResults = 0;

		public int TotalResults
		{
			get => totalResults;
			set
			{
				if (totalResults == value) return;
				totalResults = value;
				RaisePropertyChanged(nameof(TotalResults));
			}
		}

		public string SearchKey
		{
			get => searchKey;
			set
			{
				if (searchKey == value) return;
				searchKey = value;
				RaisePropertyChanged(nameof(SearchKey));
			}
		}

		public ObservableCollection<NewsModel> NewsList
		{
			get => newsList;
			set
			{
				if (newsList == value) return;
				newsList = value;
				RaisePropertyChanged(nameof(NewsList));
			}
		}

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

		public FilterModel SelectedFilterOption
		{
			get => selectedFilterOption;
			set
			{
				if (selectedFilterOption == value) return;
				selectedFilterOption = value;
				RaisePropertyChanged(nameof(SelectedFilterOption));
			}
		}

		public NewsModel SelectedNewsModel
		{
			get => selectedNewsModel;
			set
			{
				if (selectedNewsModel == value) return;
				selectedNewsModel = value;
				RaisePropertyChanged(nameof(SelectedNewsModel));
			}
		}

		public ICommand ICommandFilterOptionSelectionCommand { get; set; }
		public ICommand ICommandNewsSelectionCommand { get; set; }
		public ICommand ICommandSearchBarTapped { get; set; }
		public ICommand ICommandClearSearchTapped { get; set; }
		public ICommand ICommandFilterOptionTapped { get; set; }
		public ICommand ICommandBackButtonCommand { get; set; }

		public SearchListPageModel(INewsProviderService newsService, IFilterOptionsService filterService)
		{
			_newsService = newsService;
			_filterService = filterService;
			filterOptions = new List<FilterModel>();
			newsList = new ObservableCollection<NewsModel>();
			ICommandFilterOptionSelectionCommand = new Command<object>(FilterOptionSelectionChanged);
			ICommandNewsSelectionCommand = new Command<object>(NewsSelectionChanged);
			ICommandSearchBarTapped = new Command(async () => await SearchBarTapped());
			ICommandClearSearchTapped = new Command(() => ClearSearchTapped());
			ICommandFilterOptionTapped = new Command(async () => await FilterOptionTapped());
			ICommandBackButtonCommand = new Command(async () => await BackButtonTapped());
		}

		private void ClearSearchTapped()
		{
			SearchKey = string.Empty;
		}

		private async Task FilterOptionTapped()
		{
			var page = new BottomSheetPopup();
			await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(page);

			var result = await page.PopupClosedTask;
			if (result == null) return;
		}

		private async Task SearchBarTapped()
		{
			if (string.IsNullOrEmpty(SearchKey)) return;
			IsBusy = true;
			await RetriveNewsBySearchKey();
			IsBusy = false;
		}

		public override async void Init(object initData)
		{
			IsBusy = true;
			//InitializeFilters
			var filters = _filterService.GetFilterOptions();

			if (filters != null && filters.Any())
			{
				var firstFilter = filters.First();
				firstFilter.IsSelected = false;
				filters[0] = firstFilter;
				FilterOptions = filters;
			}

			if (initData == null) return;

			SearchKey = initData as string;
			await RetriveNewsBySearchKey();

			IsBusy = false;
			base.Init(initData);
		}

		private async void FilterOptionSelectionChanged(object selectedFilter)
		{
			if (selectedFilter != null)
			{
				FilterModel selectedItem = (FilterModel)selectedFilter;
				if (string.IsNullOrEmpty(selectedItem.FilterOption)) return;

				IsBusy = true;

				//Resetting Filter Options
				foreach (var filter in FilterOptions)
				{
					if (filter.FilterOption.ToLower() == selectedItem.FilterOption.ToLower())
						filter.IsSelected = true;
					else
						filter.IsSelected = false;
				}

				//Refeteching New list
				NewsList.Clear();
				await RetrieveNewsByCategoryList(_filterService.GetFilterOptionsEnum(selectedItem.FilterOption));

				IsBusy = false;
			}
		}

		private async Task RetrieveNewsByCategoryList(Categories category)
		{
			ArticlesResult articles = await _newsService.GetTopNewsUpdates(new TopHeadlinesRequest()
			{
				Category = category,
				Language = Languages.EN,
				Page = 1,
				PageSize = 10,
			});

			if (articles != null && articles.Articles != null && articles.Articles.Any())
			{
				var newsLists = Mapper.Map<List<Article>, List<NewsModel>>(articles.Articles);
				NewsList = newsLists.ToObservableCollection();
			}
		}

		private async Task RetriveNewsBySearchKey()
		{
			ArticlesResult articles = await _newsService.GetNewsAsync(new EverythingRequest()
			{
				Q = SearchKey,
				Language = Languages.EN,
				Page = 1,
				PageSize = 10,
				SortBy = SortBys.Relevancy,
			});

			if (articles != null && articles.Articles != null && articles.Articles.Any())
			{
				TotalResults = articles.TotalResults;
				var newsLists = Mapper.Map<List<Article>, List<NewsModel>>(articles.Articles);
				NewsList = newsLists.ToObservableCollection();
			}
		}

		private async void NewsSelectionChanged(object selectedNews)
		{
			if (selectedNews != null)
			{
				NewsModel selectedItem = (NewsModel)selectedNews;
				await CoreMethods.PushPageModel<DetailPageModel>(selectedItem);
				SelectedNewsModel = null;
			}
		}

		private async Task BackButtonTapped()
		{
			await CoreMethods.PopPageModel();
		}
	}
}