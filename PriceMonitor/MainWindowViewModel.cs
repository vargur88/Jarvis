using PriceMonitor.UI.UiViewModels;

namespace PriceMonitor
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			ShopListVM = new ShopListViewModel();
			ReportsVM = new ReportsViewModel();
			PlanetaryVM = new PlanetaryViewModel();
		}

		public ShopListViewModel ShopListVM { get; }

		public ReportsViewModel ReportsVM { get; }

		public PlanetaryViewModel PlanetaryVM { get; }

		private int _menuCount;
		public int MenuCount
		{
			get => _menuCount;
			set
			{
				_menuCount = value;
				NotifyPropertyChanged();
			}
		}
	}
}
