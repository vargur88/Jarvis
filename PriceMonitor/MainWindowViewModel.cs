using Entity.DataTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Entity;
using EveCentralProvider;

namespace PriceMonitor
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public MainWindowViewModel()
		{
			Task.Run(() =>
			{
				MenuItems = EntityService.Instance.RequestChainAsync().Result;

				var output = Services.Instance.MarketStat(new List<int>() {34}, new List<int>() { 10000014 });
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private IList<ObjectsChain> _menuItems;
		public IList<ObjectsChain> MenuItems
		{
			get { return _menuItems; }
			set
			{
				_menuItems = value;
				NotifyPropertyChanged();
			}
		}
	}
}
