using System.IO;
using System.Reflection;
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

			CreateFolders();
		}

		public void Close()
		{
			ShopListVM.Dispose();
			ReportsVM.Dispose();
			PlanetaryVM.Dispose();
		}

		private void CreateFolders()
		{
			var rootDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			if (!Directory.Exists(Path.Combine(rootDirPath, Resource1.UserData)))
			{
				Directory.CreateDirectory(Path.Combine(rootDirPath, Resource1.UserData));
			}

			if (!Directory.Exists(Path.Combine(rootDirPath, Resource1.UserData, Resource1.ShopList)))
			{
				Directory.CreateDirectory(Path.Combine(rootDirPath, Resource1.UserData, Resource1.ShopList));
			}
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
