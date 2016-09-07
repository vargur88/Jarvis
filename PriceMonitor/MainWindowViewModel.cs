using Entity.DataTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Entity;
using EveCentralProvider;
using Helpers;

namespace PriceMonitor
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public MainWindowViewModel()
		{
			Task.Run(() =>
			{
				RegionList = EntityService.Instance.RequestRegionsAsync().Result;
				FirstRegion = RegionList.First();
				SecondRegion = RegionList.First(t => t.Name.Contains("Forge"));

				MenuItems = EntityService.Instance.RequestChainAsync().Result;
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private IList<Region> _regionList;
		public IList<Region> RegionList
		{
			get { return _regionList; }
			set
			{
				_regionList = value;
				NotifyPropertyChanged();
			}
		}

		private Region _firstRegion;
		public Region FirstRegion
		{
			get { return _firstRegion; }
			set
			{
				_firstRegion = value;
				NotifyPropertyChanged();
			}
		}

		private Region _secondRegion;
		public Region SecondRegion
		{
			get { return _secondRegion; }
			set
			{
				_secondRegion = value;
				NotifyPropertyChanged();
			}
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

		private ObjectsChain _selectedChain;
		public ObjectsChain SelectedChain
		{
			get { return _selectedChain; }
			set
			{
				_selectedChain = value;
				NotifyPropertyChanged();
			}
		}

		private RelayCommand _checkPriceCmd;
		public RelayCommand CheckPriceCmd
		{
			get
			{
				return _checkPriceCmd ?? (_checkPriceCmd = new RelayCommand(p => SelectedChain != null, p => CheckPrice(SelectedChain, FirstRegion, SecondRegion)));
			}
		}

		private void CheckPrice(ObjectsChain chainToCheck, Region first, Region second)
		{
			Task.Run(async () =>
			{
				var forgePrice = await Services.Instance.MarketStatAsync(new List<int>() { chainToCheck.Object.TypeId }, new List<int>() { first.RegionId }, 1);
				var localPrice = await Services.Instance.MarketStatAsync(new List<int>() { chainToCheck.Object.TypeId }, new List<int>() { second.RegionId }, 1);


			});
		}
	}
}
