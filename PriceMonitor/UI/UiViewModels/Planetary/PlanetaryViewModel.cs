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
			if (info.Tier == PITier.Raw)
			{
				return;
			}

			var prevTierGroupView = PIGroups.SingleOrDefault(t => t.Tier == info.Tier.Prev());
			prevTierGroupView?.FocusParent(info);
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
