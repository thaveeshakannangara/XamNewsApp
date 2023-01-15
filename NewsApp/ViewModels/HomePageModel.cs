using AutoMapper;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsApp.Helpers;
using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using NewsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class HomePageModel : BaseViewModel
	{
		private readonly INewsProviderService _newsService;
		private ObservableCollection<NewsModel> topNewsList;
		private ObservableCollection<NewsModel> bottomNewsList;
		private List<string> filterOptions;
		private NewsModel selectedBottomNewsModel;
		private NewsModel selectedTopNewsModel;

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

		public ICommand ICommandSeeAllTapped { get; set; }
		public ICommand ICommandSearchBarTapped { get; set; }
		public ICommand ICommandBottomNewsSelectionCommand { get; set; }
		public ICommand ICommandTopNewsSelectionCommand { get; set; }
		public HomePageModel(INewsProviderService newsService)
		{
			_newsService = newsService;
			ICommandSeeAllTapped = new Command(async () => await SeeAllTapped());
			ICommandSearchBarTapped = new Command(async () => await SearchBarTapped());
			ICommandBottomNewsSelectionCommand = new Command<object>(BottomNewsSelectionChanged);
			ICommandTopNewsSelectionCommand = new Command<object>(TopNewsSelectionChanged);
		}

		

		public override async void Init(object initData)
		{
			IsBusy = true;
			var task1 = RetriveHotUpdates();
			var task2 = RetrieveBottomList();

			await Task.WhenAll(task1, task2);

			FilterOptions = Constants.AppContants.FilterOptions;
			base.Init(initData);
			IsBusy = false;
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

		private async Task RetrieveBottomList()
		{
			ArticlesResult articles = await _newsService.GetTopNewsUpdates(new TopHeadlinesRequest()
			{
				Category = Categories.Health,
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
			await CoreMethods.PushPageModel<SearchListPageModel>();
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

		private async Task NavigateToDetailPage(object selectedNews)
		{
			NewsModel selectedItem = (NewsModel)selectedNews;
			await CoreMethods.PushPageModel<DetailPageModel>(selectedItem);
		}

	}
}