using System;
using System.Collections.ObjectModel;
using System.Linq;
using Entity.DataTypes;
using PriceMonitor.DataTypes;

namespace PriceMonitor.UI.UiViewModels
{
	public class PIGroupViewModel : BaseViewModel
	{
		private readonly PITier _tier;

		public PIGroupViewModel(PITier tier)
		{
			_tier = tier;

			var station = new Station()
			{
				Name = "Jita",
				SystemId = 10000002,
				RegionId = 10000002
			};

			var allPi = PINode.AllPlanetaryItems.Where(t => t.Tier == tier).ToList();

			foreach (var pi in allPi)
			{
				PlanetaryWatchingItems.Add(new ItemTinyTradeHistoryViewModel(new GameObject()
				{
					Name = pi.Name,
					MarketGroupId = 0,
					TypeId = pi.ID
				}, station));
			}
		}

		private ObservableCollection<ItemTinyTradeHistoryViewModel> _planetaryWatchingItems = new ObservableCollection<ItemTinyTradeHistoryViewModel>();
		public ObservableCollection<ItemTinyTradeHistoryViewModel> PlanetaryWatchingItems
		{
			get { return _planetaryWatchingItems; }
			set
			{
				_planetaryWatchingItems = value;
				NotifyPropertyChanged();
			}
		}

		public string TierLevelName
		{
			get { return _tier.ToString(); }
			set
			{
				NotifyPropertyChanged();
			}
		}
	}
}
