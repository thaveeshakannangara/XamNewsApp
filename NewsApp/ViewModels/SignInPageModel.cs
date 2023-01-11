using FreshMvvm;
using NewsApp.Helpers;
using NewsApp.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class SignInPageModel : BaseViewModel
	{
		#region Private Variables

		private bool isPassword = true;
		private string passwordVisibilityIcon = "\uf070";
		private string email = string.Empty;
		private string password = string.Empty;

		#endregion Private Variables

		#region Public Properties

		public string Email
		{
			get => email;
			set
			{
				if (email == value) return;
				email = value;
				RaisePropertyChanged(nameof(Email));
			}
		}

		public string Password
		{
			get => password;
			set
			{
				if (password == value) return;
				password = value;
				RaisePropertyChanged(nameof(Password));
			}
		}

		public bool IsPassword
		{
			get => isPassword;
			set
			{
				if (isPassword == value) return;
				isPassword = value;
				RaisePropertyChanged(nameof(IsPassword));
			}
		}

		public string PasswordVisibilityIcon
		{
			get => passwordVisibilityIcon;
			set
			{
				if (passwordVisibilityIcon == value) return;
				passwordVisibilityIcon = value;
				RaisePropertyChanged(nameof(PasswordVisibilityIcon));
			}
		}


		#endregion Public Properties

		#region ICommands

		public ICommand ICommandPasswordVisibleClicked { get; set; }
		public ICommand ICommandForgotPasswordClicked { get; set; }
		public ICommand ICommandLoginClicked { get; set; }
		public ICommand ICommandSignUpClicked { get; set; }

		#endregion ICommands

		public SignInPageModel() // INetworkService connectivity, IDialogService service, ILocationService location
		{
			ICommandPasswordVisibleClicked = new Command(() => PasswordVisibleClicked());
			ICommandForgotPasswordClicked = new Command(async () => await ForgotPasswordClicked());
			ICommandLoginClicked = new Command(async () => await LoginClicked());
			ICommandSignUpClicked = new Command(async () => await SignUpClicked());
		}

		#region Private Methods

		private void PasswordVisibleClicked()
		{
			if (IsPassword)
			{
				PasswordVisibilityIcon = "\uf06e"; //Open Eye
				IsPassword = false;
			}
			else
			{
				PasswordVisibilityIcon = "\uf070"; //Close Eye
				IsPassword = true;
			}
		}

		private async Task ForgotPasswordClicked()
		{
			await CoreMethods.PushPageModel<ForgotPasswordPageModel>();
		}

		private async Task LoginClicked()
		{
			IsBusy = true;

			if (ValidateFields())
			{
				var page = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
				Application.Current.MainPage = new FreshNavigationContainer(page);
			}
			else
				await CoreMethods.DisplayAlert("Alert", "Please check Email and the Password", "Ok");

			IsBusy = false;
		}

		private async Task SignUpClicked()
		{
			await CoreMethods.PushPageModel<SignUpPageModel>();
		}

		private bool ValidateFields()
		{
			bool response = true;

			if (!string.IsNullOrEmpty(Email) && Email.Length > 3 && Validators.IsEmail(Email))
				response = true;
			else
			{
				response = false;
				return response;
			}

			if (!string.IsNullOrEmpty(Password) && Password.Length > 3)
				response = true;
			else
			{
				response = false;
				return response;
			}

			return response;
		}

		internal async Task BackClicked()
		{
			//var result = await CoreMethods.DisplayAlert("Exit", "Are you sure you want to exit?", "Yes", "No");
			//if (result)
			//{
			//	var closer = DependencyService.Get<ISettingsHelper>();
			//	closer?.CloseApplication();
			//}
		}

		#endregion Private Methods
	}
}