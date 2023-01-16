using FreshMvvm;
using System.Threading.Tasks;

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

		public virtual Task InitializePopupAsync()
		{
			return Task.FromResult(false);
		}
	}
}