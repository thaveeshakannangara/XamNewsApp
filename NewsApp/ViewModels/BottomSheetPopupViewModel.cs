using NewsApp.Models.AppModels;
using NewsApp.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsApp.ViewModels
{
	public class BottomSheetPopupViewModel : BaseViewModel
	{
		public object ReturnValue;
		private readonly ISortingService _sortingService;
		private List<SortingModel> sortingOptions;
		private SortingModel selectedSortingOption;

		public List<SortingModel> SortingOptions
		{
			get => sortingOptions;
			set
			{
				if (sortingOptions == value) return;
				sortingOptions = value;
				RaisePropertyChanged(nameof(SortingOptions));
			}
		}

		public SortingModel SelectedSortingOption
		{
			get => selectedSortingOption;
			set
			{
				if (selectedSortingOption == value) return;
				selectedSortingOption = value;
				RaisePropertyChanged(nameof(SelectedSortingOption));
			}
		}

		public ICommand ICommandSortingOptionSelectionCommand { get; set; }
		public ICommand ICommandSaveTappedCommand { get; set; }

		public BottomSheetPopupViewModel(ISortingService sortingService)
		{
			_sortingService = sortingService;
			ICommandSortingOptionSelectionCommand = new Command<object>(FilterOptionSelectionChanged);
			ICommandSaveTappedCommand = new Command(() => SaveTappedCommand());
			ReturnValue = null;
		}

		private void SaveTappedCommand()
		{
			Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
		}

		public override Task InitializePopupAsync()
		{
			IsBusy = true;

			var sortingList = _sortingService.GetSortingOptions();

			if (sortingList != null && sortingList.Any())
			{
				SortingOptions = sortingList;
			}

			IsBusy = false;
			return Task.FromResult(false);
		}

		private void FilterOptionSelectionChanged(object selectedSorting)
		{
			if (selectedSorting != null)
			{
				SortingModel selectedItem = (SortingModel)selectedSorting;
				if (string.IsNullOrEmpty(selectedItem.SortingOption)) return;

				IsBusy = true;

				//Resetting Filter Options
				foreach (var sorting in SortingOptions)
				{
					if (sorting.SortingOption.ToLower() == selectedItem.SortingOption.ToLower())
						sorting.IsSelected = true;
					else
						sorting.IsSelected = false;
				}

				ReturnValue = SortingOptions.FirstOrDefault(x => x.IsSelected);
				IsBusy = false;
			}
		}
	}
}