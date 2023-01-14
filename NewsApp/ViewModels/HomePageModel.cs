using NewsApp.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class HomePageModel : BaseViewModel
	{

		
		public ICommand ICommandSeeAllTapped { get; set; }
		public HomePageModel()
		{
			ICommandSeeAllTapped = new Command(async () => await SeeAllTapped());
		}

		private async Task SeeAllTapped()
		{
			await CoreMethods.PushPageModel<HotNewsPageModel>();
		}

		public override void Init(object initData)
		{
			base.Init(initData);
		}

	}
}