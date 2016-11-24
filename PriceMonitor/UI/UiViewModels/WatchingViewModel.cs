using System.Collections.Generic;
using System.Collections.ObjectModel;
using Entity.DataTypes;

namespace PriceMonitor.UI.UiViewModels
{
	public class WatchingViewModel : BaseViewModel
	{
		public WatchingViewModel()
		{
			var hubs = new List<Station>()
			{
				new Station()
				{
					Name = "Jita",
					SystemId = 10000002,
					RegionId = 10000002
				},
				new Station()
				{
					Name = "Amarr",
					SystemId = 10000043,
					RegionId = 10000043
				},
				new Station()
				{
					Name = "Hek",
					SystemId = 10000042,
					RegionId = 10000042
				}
			};

			WatchingItems.Add(new ItemTradeHistoryViewModel(GameObject.GetTritanium(), hubs));
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
