using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchListPage : ContentPage
	{
		public SearchListPage()
		{
			InitializeComponent();
		}

		private async void Button_Clicked(System.Object sender, System.EventArgs e)
		{
			try
			{
				//await Sheet.OpenSheet();
			}
			catch (Exception ex)
			{
				//ex.Log();
			}
		}

	}
}