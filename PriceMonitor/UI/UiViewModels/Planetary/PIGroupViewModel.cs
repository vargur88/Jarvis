using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Entity.DataTypes;
using PriceMonitor.DataTypes;
using EveCentralProvider;
using System.Threading.Tasks;

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

			var allTierPi = PINode.AllPlanetaryItems.Where(t => t.Tier == tier).ToList();

			this._planetaryViewModel = planetaryViewModel;
			this.Tier = tier;

			var piListId = new List<int>();
			foreach (var pi in allTierPi)
			{
				piListId.Add(pi.ID);

				PlanetaryWatchingItems.Add(new ItemTinyTradeHistoryViewModel(
					_planetaryViewModel,
					tier,
					station,
					new GameObject()
					{
						Name = pi.Name,
						MarketGroupId = 0,
						TypeId = pi.ID
					}));
			}

			string PriceConvert(float sellPrice, float buyPrice)
			{
				switch(tier)
				{
					case PITier.Raw:
					case PITier.Basic:
					case PITier.Refined:
						return $"{Math.Round(sellPrice)}/{Math.Round(buyPrice)}";
					default:
						return $"{Math.Round(sellPrice / 1000000, 4)}/{Math.Round(buyPrice / 1000000, 4)}";
				}
			}

			Task.Run(async () =>
			{
				await Services.Instance.MarketStatAsync(piListId, new List<int>() { station.RegionId }, 100)
				.ContinueWith(t =>
				{
					if (t.Status == TaskStatus.RanToCompletion && t.Result != null)
					{
						foreach (var item in t.Result)
						{
							PlanetaryWatchingItems.Single(g => g.GameObject.TypeId == item.Id).Price =
								PriceConvert(item.Sell.Percentile, item.Buy.Percentile);
						}
					}
				})
				.ConfigureAwait(false);
			});
		}

		private ObservableCollection<ItemTinyTradeHistoryViewModel> _planetaryWatchingItems = new ObservableCollection<ItemTinyTradeHistoryViewModel>();

		public ObservableCollection<ItemTinyTradeHistoryViewModel> PlanetaryWatchingItems
		{
			get => _planetaryWatchingItems;
			set
			{
				_planetaryWatchingItems = value;
				NotifyPropertyChanged();
			}
		}

		public string TierLevelName
		{
			get => Tier.ToString();
			set => NotifyPropertyChanged();
		}

		public void Focusing(PlanetaryViewModel.PIObserveInfo info)
		{
			// non optimal
			var childList = PINode.AllPlanetaryItems.Where(t => t.Tier != PITier.Raw && t.From.Any(k => k == info.PiID)).ToList();
			var parentList = PINode.AllPlanetaryItems.Where(t => t.Tier != PITier.Advanced && t.To.Any(k => k == info.PiID)).ToList();

			var modelChilds1 = PlanetaryWatchingItems.Where(t => childList.Any(k => k.ID == t.GameObject.TypeId) || parentList.Any(k => k.ID == t.GameObject.TypeId));
			foreach (var model in modelChilds1)
			{
				model.Focusing(info);
			}
		}

		public void SelectChilds(PlanetaryViewModel.PIObserveInfo info)
		{
			var childList = PINode.AllPlanetaryItems.Where(t => t.From.Any(k => k == info.PiID)).ToList();

			var models = childList.Select(b => PlanetaryWatchingItems.Single(t => t.GameObject.TypeId == b.ID)).ToList();

			foreach (var model in models)
			{
				model.UpdatePiChain(info);
			}
		}

		public void FocusParent(PlanetaryViewModel.PIObserveInfo info)
		{
			var parentList = PINode.AllPlanetaryItems.Where(t => t.Tier != PITier.Advanced && t.To.Any(k => k == info.PiID)).ToList();

			var models = parentList.Select(b => PlanetaryWatchingItems.Single(t => t.GameObject.TypeId == b.ID)).ToList();

			foreach (var model in models)
			{
				model.ChangeFocus(info);
			}
		}
	}
}
