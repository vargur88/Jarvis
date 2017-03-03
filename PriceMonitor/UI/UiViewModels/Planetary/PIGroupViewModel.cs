using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Entity.DataTypes;
using PriceMonitor.DataTypes;

namespace PriceMonitor.UI.UiViewModels
{
	public class PIGroupViewModel : BaseViewModel
	{
		public readonly PITier Tier;
		private readonly PlanetaryViewModel _planetaryViewModel;

		public PIGroupViewModel(PlanetaryViewModel planetaryViewModel, PITier tier)
		{
			Tier = tier;

			var station = new Station()
			{
				Name = "Jita",
				SystemId = 10000002,
				RegionId = 10000002
			};

			var allPi = PINode.AllPlanetaryItems.Where(t => t.Tier == tier).ToList();

			this._planetaryViewModel = planetaryViewModel;
			this.Tier = tier;

			foreach (var pi in allPi)
			{
				PlanetaryWatchingItems.Add(new ItemTinyTradeHistoryViewModel(
					_planetaryViewModel,
					tier,
					new GameObject()
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
			get { return Tier.ToString(); }
			set
			{
				NotifyPropertyChanged();
			}
		}

		public void SelectChilds(PlanetaryViewModel.PIObserveInfo info)
		{
			var childList = PINode.AllPlanetaryItems.SelectMany(t => t.From.Where(k => k == info.PiID)).ToList();

			List<ItemTinyTradeHistoryViewModel> l = new List<ItemTinyTradeHistoryViewModel>();

			foreach (var b in childList)
			{
				l.AddRange(PlanetaryWatchingItems.Where(t => t.GameObject.TypeId == b).ToList());
			}

			//var watchItems = PlanetaryWatchingItems.All(t => t.GameObject.TypeId == )
		}
	}
}
