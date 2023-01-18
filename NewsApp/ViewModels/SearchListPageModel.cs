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
		private readonly ISortingService _sortingService;
		private List<FilterModel> filterOptions;
		private FilterModel selectedFilterOption;
		private NewsModel selectedNewsModel;
		private ObservableCollection<NewsModel> newsList;
		private string searchKey = string.Empty;
		private int totalResults = 0;
		private SortBys sortOption;
		private int itemTreshold;
		private int currentPage;
		private const int pageSize = 10;

		public int ItemTreshold
		{
			get => itemTreshold;
			set
			{
				if (itemTreshold == value) return;
				itemTreshold = value;
				RaisePropertyChanged(nameof(ItemTreshold));
			}
		}

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
		public ICommand ItemTresholdReachedCommand { get; set; }

		public SearchListPageModel(INewsProviderService newsService, IFilterOptionsService filterService,
			ISortingService sortingService)
		{
			_newsService = newsService;
			_filterService = filterService;
			_sortingService = sortingService;
			ItemTreshold = 1;
			currentPage = 1;
			sortOption = SortBys.Relevancy;
			filterOptions = new List<FilterModel>();
			newsList = new ObservableCollection<NewsModel>();
			ICommandFilterOptionSelectionCommand = new Command<object>(FilterOptionSelectionChanged);
			ICommandNewsSelectionCommand = new Command<object>(NewsSelectionChanged);
			ICommandSearchBarTapped = new Command(async () => await SearchBarTapped());
			ICommandClearSearchTapped = new Command(() => ClearSearchTapped());
			ICommandFilterOptionTapped = new Command(async () => await FilterOptionTapped());
			ICommandBackButtonCommand = new Command(async () => await BackButtonTapped());
			ItemTresholdReachedCommand = new Command(async () => await ItemsTresholdReached());
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

			var sorting = (SortingModel)result;
			IsBusy = true;
			sortOption = _sortingService.GetSortingOptionsEnum(sorting.SortingOption);
			await RetriveNewsBySearchKey();
			IsBusy = false;
		}

		private async Task ItemsTresholdReached()
		{
			currentPage++;
			IsBusy = true;
			await RetriveNewsBySearchKey();
			IsBusy = false;
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
				PageSize = pageSize,
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
				Page = currentPage,
				PageSize = pageSize,
				SortBy = sortOption,
			});

			if (articles != null && articles.Articles != null && articles.Articles.Any())
			{
				TotalResults = articles.TotalResults;
				var newsLists = Mapper.Map<List<Article>, List<NewsModel>>(articles.Articles);

				foreach (var item in newsLists)
				{
					NewsList.Add(item);
				}

				if (articles.TotalResults < (currentPage * pageSize))
				{
					ItemTreshold = -1;
				}
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