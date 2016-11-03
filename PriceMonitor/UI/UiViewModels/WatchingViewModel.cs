using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EveCentralProvider;

namespace PriceMonitor.UI.UiViewModels
{
	public class WatchingViewModel : BaseViewModel
	{
		public WatchingViewModel()
		{
			WatchingItems.Add(new ItemTradeHistoryViewModel());
			WatchingItems.Add(new ItemTradeHistoryViewModel());
			WatchingItems.Add(new ItemTradeHistoryViewModel());
			WatchingItems.Add(new ItemTradeHistoryViewModel());

			Task.Run(async () =>
			{
				var k = await Services.Instance.HistoryAsync(2865, 10000002);
			}).Wait();
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
