using PriceMonitor.UI.UiViewModels;

namespace PriceMonitor
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			WatchingVM = new WatchingViewModel();
			ReportsVM = new ReportsViewModel();
			PlanetaryVM = new PlanetaryViewModel();
		}

		public WatchingViewModel WatchingVM { get; private set; }

		public ReportsViewModel ReportsVM { get; private set; }

		public PlanetaryViewModel PlanetaryVM { get; private set; }

		private int _menuCount;
		public int MenuCount
		{
			get { return _menuCount; }
			set
			{
				_menuCount = value;
				NotifyPropertyChanged();
			}
		}
	}
}
