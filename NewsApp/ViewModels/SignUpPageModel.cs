using NewsApp.Helpers;
using NewsApp.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp
{
	public class SignUpPageModel : BaseViewModel
	{
		#region Private Variables

		private bool isPassword = true;
		private string passwordVisibilityIcon = "\uf070";
		private bool isAgreeToTOS = false;

		private string firstName = string.Empty;
		private string lastName = string.Empty;
		private string email = string.Empty;
		private string password = string.Empty;

		private string validateResponse = string.Empty;

		#endregion Private Variables

		#region Public Properties

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

		public bool IsAgreeToTOS
		{
			get => isAgreeToTOS;
			set
			{
				if (isAgreeToTOS == value) return;
				isAgreeToTOS = value;
				RaisePropertyChanged(nameof(IsAgreeToTOS));
			}
		}

		public string FirstName
		{
			get => firstName;
			set
			{
				if (firstName == value) return;
				firstName = value;
				RaisePropertyChanged(nameof(FirstName));
			}
		}

		public string LastName
		{
			get => lastName;
			set
			{
				if (lastName == value) return;
				lastName = value;
				RaisePropertyChanged(nameof(LastName));
			}
		}

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

		#endregion Public Properties

		#region ICommands

		public ICommand ICommandPasswordVisibleClicked { get; set; }
		public ICommand ICommandSignUpClicked { get; set; }
		public ICommand ICommandSignInClicked { get; set; }
		public ICommand ICommandTermsOfServicesClicked { get; set; }
		public ICommand ICommandPrivacyPolicyClicked { get; set; }

		#endregion ICommands

		#region Constructor

		public SignUpPageModel()
		{
			ICommandPasswordVisibleClicked = new Command(() => PasswordVisibleClicked());
			ICommandSignUpClicked = new Command(async () => await SignUpClicked());
			ICommandSignInClicked = new Command(async () => await SignInClicked());
			ICommandTermsOfServicesClicked = new Command(async () => await TermsOfServicesClicked());
			ICommandPrivacyPolicyClicked = new Command(async () => await PrivacyPolicyClicked());
		}

		#endregion Constructor

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

		private async Task SignUpClicked()
		{
			IsBusy = true;

			if (string.IsNullOrEmpty(ValidateFields()))
			{
				if (IsAgreeToTOS)
				{
					await CoreMethods.PopToRoot(animate: true);
				}
				else
					await CoreMethods.DisplayAlert("Alert", "Please check & agree to the TOS / Privacy Policy", "Ok");
			}
			else
				await CoreMethods.DisplayAlert("Alert", validateResponse, "Ok");

			IsBusy = false;
		}

		private async Task SignInClicked()
		{
			await CoreMethods.PopToRoot(animate: true);
		}

		private async Task TermsOfServicesClicked()
		{
			await CoreMethods.DisplayAlert("Alert", "TermsofServices Tapped", "Ok");
		}

		private async Task PrivacyPolicyClicked()
		{
			await CoreMethods.DisplayAlert("Alert", "Privacy Policy Tapped", "Ok");
		}

		private string ValidateFields()
		{
			validateResponse = string.Empty;
			if (string.IsNullOrEmpty(FirstName) && FirstName.Length < 4)
			{
				validateResponse = "First Name shouldn't be empty or should be greater than 3 letters";
				return validateResponse;
			}

			if (string.IsNullOrEmpty(LastName) && LastName.Length < 4)
			{
				validateResponse = "Last Name shouldn't be empty or should be greater than 3 letters";
				return validateResponse;
			}

			if (string.IsNullOrEmpty(Email) && Email.Length < 4)
			{
				validateResponse = "Email shouldn't be empty or should be greater than 3 letters";
				return validateResponse;
			}

			if (!Validators.IsEmail(Email))
			{
				validateResponse = "Email should be in correct format";
				return validateResponse;
			}

			if (string.IsNullOrEmpty(Password) && Password.Length < 4)
			{
				validateResponse = "Password shouldn't be empty or should be greater than 3 letters";
				return validateResponse;
			}

			return validateResponse;
		}

		#endregion Private Methods
	}
}