using System.Collections.ObjectModel;
using Entity.DataTypes;

namespace PriceMonitor.UI.UiViewModels
{
	public class PlanetaryViewModel : BaseViewModel
	{
		public PlanetaryViewModel()
		{
			var station = new Station()
				{
					Name = "Jita",
					SystemId = 10000002,
					RegionId = 10000002
				};

			for (int i = 0; i < 20; ++i)
			{
				PlanetaryWatchingItems.Add(new ItemTinyTradeHistoryViewModel(GameObject.GetTritanium(), station));
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
	}
}
