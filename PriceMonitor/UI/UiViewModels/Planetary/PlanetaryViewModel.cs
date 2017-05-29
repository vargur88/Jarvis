using System.Collections.ObjectModel;
using PriceMonitor.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using PriceMonitor.Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class PlanetaryViewModel : BaseViewModel
	{
		public PlanetaryViewModel()
		{
			foreach (PITier tier in Enum.GetValues(typeof(PITier)))
			{
				var view = new PIGroupViewModel(this, tier);
				_dictView[tier] = view;
				PIGroups.Add(view);
			}
		}

		private Dictionary<PITier, PIGroupViewModel> _dictView = new Dictionary<PITier, PIGroupViewModel>();

		private ObservableCollection<PIGroupViewModel> _piGroups = new ObservableCollection<PIGroupViewModel>();
		public ObservableCollection<PIGroupViewModel> PIGroups
		{
			get => _piGroups;
			set
			{
				_piGroups = value;
				NotifyPropertyChanged();
			}
		}

		private readonly List<ItemTinyTradeHistoryViewModel> _prevSelected = new List<ItemTinyTradeHistoryViewModel>();
		public void PIFocusing(PIObserveInfo info)
		{
			if (!info.InFocus)
			{
				foreach (var view in _prevSelected)
				{
					view.ChangeFocus2(false);
				}
				_prevSelected.Clear();
				return;
			}

			Dictionary<PITier, List<int>> list = new Dictionary<PITier, List<int>>()
			{
				{ info.Tier, new List<int>(){info.PiID}}
			};

			BuildPIChain(true, info.Tier, list.First().Value, list);
			BuildPIChain(false, info.Tier, list.First().Value, list);

			foreach (var group in _dictView)
			{
				if (!list.ContainsKey(group.Key))
				{
					continue;
				}

				var views = group.Value.PlanetaryWatchingItems.Where(t => list[group.Key].Any(k => k == t.GameObject.TypeId)).ToList();
				foreach (var view in views)
				{
					view.ChangeFocus2(true);
					_prevSelected.Add(view);
				}
			}

			/*
			PIGroupViewModel prevTierGroupView = null;
			PIGroupViewModel nextTierGroupView = null;

			if (info.Tier != PITier.Raw)
			{
				prevTierGroupView = PIGroups.SingleOrDefault(t => t.Tier == info.Tier.Prev());
			}

			if (info.Tier != PITier.Advanced)
			{
				nextTierGroupView = PIGroups.SingleOrDefault(t => t.Tier == info.Tier.Next());
			}

			prevTierGroupView?.Focusing(info);
			nextTierGroupView?.Focusing(info);*/
		}

		private void BuildPIChain(bool directionToHigh, PITier tier, List<int> piList, Dictionary<PITier, List<int>> result)
		{
			foreach (var id in piList)
			{
				var node = PINode.AllPlanetaryItems.Single(t => t.ID == id);

				if (directionToHigh)
				{
					if (tier != PITier.Advanced)
					{
						var childs = PINode.AllPlanetaryItems
							.Where(t => node.To.Any(k => k == t.ID))
							.Select(t => t.ID)
							.ToList();

						var nextTier = tier.Next();

						if (!result.ContainsKey(nextTier))
						{
							result[nextTier] = new List<int>();
						}
						result[nextTier].AddRange(childs);

						BuildPIChain(true, nextTier, childs, result);
					}
				}
				else if (tier != PITier.Raw)
				{
					var childs = PINode.AllPlanetaryItems
						.Where(t => node.From.Any(k => k == t.ID))
						.Select(t => t.ID)
						.ToList();

					var prevTier = tier.Prev();

					if (!result.ContainsKey(prevTier))
					{
						result[prevTier] = new List<int>();
					}
					result[prevTier].AddRange(childs);

					BuildPIChain(false, prevTier, childs, result);
				}
			}
		}

		public void PIObserving(PIObserveInfo info)
		{
			if (info.Tier == PITier.Advanced)
			{
				return;
			}

			var nextTierGroupView = PIGroups.SingleOrDefault(t => t.Tier == info.Tier.Next());
			nextTierGroupView?.SelectChilds(info);
		}

		public struct PIObserveInfo
		{
			public int PiID;
			public Brush ParentBrush;
			public bool CreatePiChain;
			public PITier Tier;
			public bool InFocus;
		}
	}
}
