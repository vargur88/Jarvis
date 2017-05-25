using System.Collections.ObjectModel;
using PriceMonitor.DataTypes;
using System;
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
				PIGroups.Add(new PIGroupViewModel(this, tier));
			}
		}

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

		public void PIFocusing(PIObserveInfo info)
		{
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
			nextTierGroupView?.Focusing(info);
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
