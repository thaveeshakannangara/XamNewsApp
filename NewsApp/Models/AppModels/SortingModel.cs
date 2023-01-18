using Xamarin.CommunityToolkit.ObjectModel;

namespace NewsApp.Models.AppModels
{
	public class SortingModel : ObservableObject
	{
		private bool isSelected = false;
		public string SortingOption { get; set; }

		public bool IsSelected
		{
			get => isSelected;
			set
			{
				if (isSelected == value) return;
				isSelected = value;
				OnPropertyChanged(nameof(IsSelected));
			}
		}
	}
}