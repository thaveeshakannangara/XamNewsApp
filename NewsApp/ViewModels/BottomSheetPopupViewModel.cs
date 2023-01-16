using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.ViewModels
{
	public class BottomSheetPopupViewModel : BaseViewModel
	{
		public object ReturnValue;

		public BottomSheetPopupViewModel()
		{
				
		}

		public override Task InitializePopupAsync()
		{
			return Task.FromResult(false);
		}
	}
}
