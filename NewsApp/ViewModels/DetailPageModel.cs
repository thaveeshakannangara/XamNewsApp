using NewsApp.Models.AppModels;
using NewsApp.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class DetailPageModel : BaseViewModel
	{
		private NewsModel newsModel;
		public NewsModel NewsModel
		{
			get => newsModel;
			set
			{
				if (newsModel == value) return;
				newsModel = value;
				RaisePropertyChanged(nameof(NewsModel));
			}
		}

		public ICommand ICommandBackButtonCommand { get; set; }

		public DetailPageModel()
		{
			ICommandBackButtonCommand = new Command(async () => await BackButtonTapped());
		}

		public override void Init(object initData)
		{
			if (initData != null)
			{
				NewsModel = (NewsModel)initData;
			}

			base.Init(initData);
		}

		private async Task BackButtonTapped()
		{
			await CoreMethods.PopPageModel();
		}
	}
}