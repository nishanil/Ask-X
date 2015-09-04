using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AskXhacker
{
	public class ObservableObject : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		}
	}
}

