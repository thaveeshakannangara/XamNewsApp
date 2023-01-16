using AutoMapper;
using FreshMvvm;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsApp.Constants;
using NewsApp.Helpers;
using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using NewsApp.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewsApp
{
	public class HomePageModel : BaseViewModel
	{
		private readonly INewsProviderService _newsService;
		private readonly IFilterOptionsService _filterService;
		private ObservableCollection<NewsModel> topNewsList;
		private ObservableCollection<NewsModel> bottomNewsList;
		private List<FilterModel> filterOptions;
		private NewsModel selectedBottomNewsModel;
		private NewsModel selectedTopNewsModel;
		private FilterModel selectedFilterOption;
		private string searchKey = string.Empty;
		private bool isActivityBusy = false;

		public bool IsActivityBusy
		{
			get => isActivityBusy;
			set
			{
				if (isActivityBusy == value) return;
				isActivityBusy = value;
				RaisePropertyChanged(nameof(IsActivityBusy));
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

		public ObservableCollection<NewsModel> TopNewsList
		{
			get => topNewsList;
			set
			{
				if (topNewsList == value) return;
				topNewsList = value;
				RaisePropertyChanged(nameof(TopNewsList));
			}
		}

		public ObservableCollection<NewsModel> BottomNewsList
		{
			get => bottomNewsList;
			set
			{
				if (bottomNewsList == value) return;
				bottomNewsList = value;
				RaisePropertyChanged(nameof(BottomNewsList));
			}
		}

		public NewsModel SelectedBottomNewsModel
		{
			get => selectedBottomNewsModel;
			set
			{
				if (selectedBottomNewsModel == value) return;
				selectedBottomNewsModel = value;
				RaisePropertyChanged(nameof(SelectedBottomNewsModel));
			}
		}

		public NewsModel SelectedTopNewsModel
		{
			get => selectedTopNewsModel;
			set
			{
				if (selectedTopNewsModel == value) return;
				selectedTopNewsModel = value;
				RaisePropertyChanged(nameof(SelectedTopNewsModel));
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

		public ICommand ICommandSeeAllTapped { get; set; }
		public ICommand ICommandSearchBarTapped { get; set; }
		public ICommand ICommandBottomNewsSelectionCommand { get; set; }
		public ICommand ICommandTopNewsSelectionCommand { get; set; }
		public ICommand ICommandFilterOptionSelectionCommand { get; set; }
		public ICommand ICommandLogoutCommand { get; set; }

		public HomePageModel(INewsProviderService newsService, IFilterOptionsService filterService)
		{
			_newsService = newsService;
			_filterService = filterService;
			filterOptions = new List<FilterModel>();
			ICommandSeeAllTapped = new Command(async () => await SeeAllTapped());
			ICommandSearchBarTapped = new Command(async () => await SearchBarTapped());
			ICommandBottomNewsSelectionCommand = new Command<object>(BottomNewsSelectionChanged);
			ICommandTopNewsSelectionCommand = new Command<object>(TopNewsSelectionChanged);
			ICommandFilterOptionSelectionCommand = new Command<object>(FilterOptionSelectionChanged);
			ICommandLogoutCommand = new Command(async () => await OnLogoutTapped());
		}

		public override async void Init(object initData)
		{
			IsBusy = IsActivityBusy = true;
			var task1 = RetriveHotUpdates();
			var task2 = InitializeBottomList();

			await Task.WhenAll(task1, task2);

			FilterOptions = _filterService.GetFilterOptions();
			base.Init(initData);
			IsBusy = IsActivityBusy = false;
		}

		private async Task RetriveHotUpdates()
		{
			ArticlesResult articles = await _newsService.GetTopNewsUpdates(new TopHeadlinesRequest()
			{
				Category = Categories.Business,
				Language = Languages.EN,
				Page = 1,
				PageSize = 5,
			});

			if (articles != null && articles.Articles != null && articles.Articles.Any())
			{
				var newsList = Mapper.Map<List<Article>, List<NewsModel>>(articles.Articles);
				TopNewsList = newsList.ToObservableCollection();
			}
		}

		private async Task InitializeBottomList()
		{
			await RetrieveBottomList(Categories.Health);
		}

		private async Task RetrieveBottomList(Categories category)
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
				var newsList = Mapper.Map<List<Article>, List<NewsModel>>(articles.Articles);
				BottomNewsList = newsList.ToObservableCollection();
			}
		}

		private async Task SearchBarTapped()
		{
			if (string.IsNullOrEmpty(SearchKey)) return;

			await CoreMethods.PushPageModel<SearchListPageModel>(SearchKey);
		}

		private async Task SeeAllTapped()
		{
			await CoreMethods.PushPageModel<HotNewsPageModel>();
		}

		private async void BottomNewsSelectionChanged(object selectedNews)
		{
			if (selectedNews != null)
			{
				await NavigateToDetailPage(selectedNews);
				SelectedBottomNewsModel = null;
			}
		}

		private async void TopNewsSelectionChanged(object selectedNews)
		{
			if (selectedNews != null)
			{
				await NavigateToDetailPage(selectedNews);
				SelectedTopNewsModel = null;
			}
		}

		private async void FilterOptionSelectionChanged(object selectedFilter)
		{
			if (selectedFilter != null)
			{
				FilterModel selectedItem = (FilterModel)selectedFilter;
				if (string.IsNullOrEmpty(selectedItem.FilterOption)) return;

				IsActivityBusy = true;

				//Resetting Filter Options
				foreach (var filter in FilterOptions)
				{
					if (filter.FilterOption.ToLower() == selectedItem.FilterOption.ToLower())
						filter.IsSelected = true;
					else
						filter.IsSelected = false;
				}

				//Refeteching New list
				BottomNewsList.Clear();
				await RetrieveBottomList(_filterService.GetFilterOptionsEnum(selectedItem.FilterOption));

				IsActivityBusy = false;
			}
		}

		private async Task NavigateToDetailPage(object selectedNews)
		{
			NewsModel selectedItem = (NewsModel)selectedNews;
			await CoreMethods.PushPageModel<DetailPageModel>(selectedItem);
		}

		private async Task OnLogoutTapped()
		{
			bool isLogout = await Application.Current.MainPage.DisplayAlert("Logout", "Do you want to Logout from the app", "Yes", "No");
			if (isLogout)
			{
				Preferences.Set(PreferencesKey.IsLoggedIn, false);
				Preferences.Set(PreferencesKey.IsUserEmail, string.Empty);
				var page = FreshPageModelResolver.ResolvePageModel<SignInPageModel>();
				Application.Current.MainPage = new FreshNavigationContainer(page);
			}
		}
	}
}