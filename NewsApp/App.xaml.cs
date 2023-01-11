using FreshMvvm;
using Xamarin.Forms;

namespace NewsApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			RegisterServiceInIOCContainer();
			App.Current.UserAppTheme = OSAppTheme.Light;
			var page = FreshPageModelResolver.ResolvePageModel<SignInPageModel>();  //HomePageModel   SignInPageModel MainPageModel
			MainPage = new FreshNavigationContainer(page);
		}

		/// <summary>
		/// Add here to Register Services in the Dependency Container
		/// </summary>
		private void RegisterServiceInIOCContainer()
		{
			//FreshIOC.Container.Register<ISettings, EnsoUITemplate.Helpers.Settings>();
			//FreshIOC.Container.Register<IDialogService, DialogService>();
			//FreshIOC.Container.Register<INetworkService, NetworkService>();
			//FreshIOC.Container.Register<ILocationService, LocationService>();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}