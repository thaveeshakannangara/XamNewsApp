using FreshMvvm;

namespace NewsApp.ViewModels
{
	public class BaseViewModel : FreshBasePageModel
	{
		private bool isBusy = false;

		public bool IsBusy
		{
			get => isBusy;
			set
			{
				if (isBusy == value) return;
				isBusy = value;
				RaisePropertyChanged(nameof(IsBusy));
			}
		}
	}
}