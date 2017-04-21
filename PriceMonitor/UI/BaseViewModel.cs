using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PriceMonitor
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		protected BaseViewModel()
		{
			IsUiAvailable = true;
		}

		private bool _uiAvailable;
		public bool IsUiAvailable
		{
			get => _uiAvailable;
			set
			{
				_uiAvailable = value;
				NotifyPropertyChanged();
			}
		}

		public virtual void Refresh() { }

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
