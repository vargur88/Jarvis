using System.Collections.ObjectModel;

namespace PriceMonitor.UI.UiViewModels
{
	public class WatchingViewModel : BaseViewModel
	{
		public WatchingViewModel()
		{
			WatchingItems.Add(new ItemTradeHistoryViewModel());
		}

		private ObservableCollection<ItemTradeHistoryViewModel> _watchingItems = new ObservableCollection<ItemTradeHistoryViewModel>();
		public ObservableCollection<ItemTradeHistoryViewModel> WatchingItems
		{
			get { return _watchingItems; }
			set
			{
				_watchingItems = value;
				NotifyPropertyChanged();
			}
		}
	}
}
