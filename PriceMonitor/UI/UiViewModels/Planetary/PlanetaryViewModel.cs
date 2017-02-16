using System.Collections.ObjectModel;
using Entity.DataTypes;
using PriceMonitor.DataTypes;
using System;

namespace PriceMonitor.UI.UiViewModels
{
	public class PlanetaryViewModel : BaseViewModel
	{
		public PlanetaryViewModel()
		{
			foreach (PITier tier in Enum.GetValues(typeof(PITier)))
			{
				PIGroups.Add(new PIGroupViewModel(tier));
			}
		}

		private ObservableCollection<PIGroupViewModel> _piGroups = new ObservableCollection<PIGroupViewModel>();
		public ObservableCollection<PIGroupViewModel> PIGroups
		{
			get { return _piGroups; }
			set
			{
				_piGroups = value;
				NotifyPropertyChanged();
			}
		}
	}
}
