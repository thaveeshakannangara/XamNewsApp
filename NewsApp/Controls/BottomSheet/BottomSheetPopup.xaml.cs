using NewsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsApp.Controls.BottomSheet
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BottomSheetPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		
		private TaskCompletionSource<object> _taskCompletionSource;
		public Task<object> PopupClosedTask => _taskCompletionSource.Task;

		public BottomSheetPopup()
		{
			InitializeComponent();

			this.BindingContext = new BottomSheetPopupViewModel();
			new Task(new Action(async () =>
			{
				await (this.BindingContext as BaseViewModel).InitializePopupAsync();

			})).Start();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_taskCompletionSource = new TaskCompletionSource<object>();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			_taskCompletionSource.SetResult(((BottomSheetPopupViewModel)BindingContext).ReturnValue);
		}

		// ### Overrided methods which can prevent closing a popup page ###

		// Invoked when a hardware back button is pressed
		protected override bool OnBackButtonPressed()
		{
			// Return true if you don't want to close this popup page when a back button is pressed
			return base.OnBackButtonPressed();
		}

		// Invoked when background is clicked
		protected override bool OnBackgroundClicked()
		{
			// Return false if you don't want to close this popup page when a background of the popup page is clicked
			return base.OnBackgroundClicked();
		}
	}
}