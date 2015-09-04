using System;

namespace AskXhacker
{
	public class ViewModelBase : ObservableObject
	{
		private bool isBusy;
		public bool IsBusy {
			get{ return isBusy; }
			set{ isBusy = value; RaisePropertyChanged (); }
		}

		private string title;
		public string Title {
			get{ return title; }
			set{ title = value; RaisePropertyChanged (); }
		}
	}


}

