using AutoMapper;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsApp.Helpers;
using NewsApp.Models.AppModels;
using NewsApp.Services.ApiServices.Contracts;
using NewsApp.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class HotNewsPageModel : BaseViewModel
	{
		private readonly INewsProviderService _newsService;
		private ObservableCollection<NewsModel> topNewsList;

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

		public ICommand ICommandBackButtonCommand { get; set; }

		public HotNewsPageModel(INewsProviderService newsService)
		{
			_newsService = newsService;
			topNewsList = new ObservableCollection<NewsModel>();
			ICommandBackButtonCommand = new Command(async () => await BackButtonTapped());
		}

		public override async void Init(object initData)
		{
			IsBusy = true;
			await RetriveHotUpdates();
			base.Init(initData);
			IsBusy = false;
		}

		private async Task RetriveHotUpdates()
		{
			ArticlesResult articles = await _newsService.GetTopNewsUpdates(new TopHeadlinesRequest()
			{
				Category = Categories.Technology,
				Language = Languages.EN,
				Page = 1,
				PageSize = 10,
			});

			if (articles != null && articles.Articles != null && articles.Articles.Any())
			{
				var newsList = Mapper.Map<List<Article>, List<NewsModel>>(articles.Articles);
				TopNewsList = newsList.ToObservableCollection();
			}
		}

		private async Task BackButtonTapped()
		{
			await CoreMethods.PopPageModel();
		}

	}
}