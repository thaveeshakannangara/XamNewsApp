using FreshMvvm;
using NewsApp.Constants;
using NewsApp.Data;
using NewsApp.Data.Contracts;
using NewsApp.Data.Repositories;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NewsApp
{
	public partial class App : Application
	{
		private static NewsSqliteDatabase database;

		public App()
		{
			InitializeComponent();

			InitDIContainer();
			InitDatabase();
			App.Current.UserAppTheme = OSAppTheme.Light;
			InitNavigation();
		}

		/// <summary>
		/// Add here to Register Services in the Dependency Container
		/// </summary>
		private void InitDIContainer()
		{
			FreshIOC.Container.Register<ISqliteConnectionManager, SqliteConnectionManager>();
			FreshIOC.Container.Register<IUserRepository, UserRepository>();
		}

		private static void InitDatabase()
		{
			if (database == null)
			{
				var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppContants.DbName);
				Preferences.Set(PreferencesKey.DatabasePath, dbPath);
				SecureStorage.SetAsync(PreferencesKey.DbKey, AppContants.DbSecretKey).GetAwaiter().GetResult();
				database = new NewsSqliteDatabase(dbPath);
			}
		}

		private void InitNavigation()
		{
			bool isLoggedIn = Preferences.Get(PreferencesKey.IsLoggedIn, false);

			if (!isLoggedIn)
			{
				var page = FreshPageModelResolver.ResolvePageModel<SignInPageModel>();
				MainPage = new FreshNavigationContainer(page);
			}
			else
			{
				var page = FreshPageModelResolver.ResolvePageModel<HotNewsPageModel>();//HomePageModel
				MainPage = new FreshNavigationContainer(page);
			}
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