using System;
using PriceMonitor.DataTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Entity.DataTypes;
using System.Linq;
using Entity;
using EveCentralProvider;
using EveCentralProvider.Types;
using Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class ChartViewModel : BaseViewModel
	{
		public ChartViewModel(List<Region> regionList)
		{
			RegionList = regionList;

			Task.Run(() =>
			{
				SelectedRegion = RegionList.Single(t => t.RegionId == 10000002); // The Forge

				SystemList = EntityService.Instance.RequestSystemsByRegionAsync(SelectedRegion.RegionId).Result;
				SelectedSystem = SystemList.Single(t => t.SystemId == 30000142); // Jita

				StationList = EntityService.Instance.RequestStationsBySystemAsync(SelectedSystem.SystemId).Result;
				SelectedStation = StationList.Single(t => t.StationId == 60003760); //Jita IV - Moon 4 - Caldari Navy Assembly Plant
			});
		}

		// Regions properties
		public List<Region> RegionList { get; set; }

		private Region _selectedRegion;
		public Region SelectedRegion
		{
			get { return _selectedRegion; }
			set
			{
				if (_selectedRegion == value || value == null)
				{
					return;
				}
				_selectedRegion = value;
				NotifyPropertyChanged();

				SystemList = EntityService.Instance.RequestSystemsByRegionAsync(value.RegionId).Result;
				if (SystemList.Count > 0)
				{
					SelectedSystem = SystemList.First();
				}
			}
		}

		// System properties
		private IList<SolarSystem> _systemList;
		public IList<SolarSystem> SystemList
		{
			get { return _systemList; }
			set
			{
				_systemList = value;
				NotifyPropertyChanged();
			}
		}

		private SolarSystem _selectedSystem;
		public SolarSystem SelectedSystem
		{
			get { return _selectedSystem; }
			set
			{
				if (_selectedSystem == value || value == null)
				{
					return;
				}
				_selectedSystem = value;
				NotifyPropertyChanged();

				StationList = EntityService.Instance.RequestStationsBySystemAsync(value.SystemId).Result;
				if (StationList.Count > 0)
				{
					SelectedStation = StationList.First();
				}
			}
		}

		// Stations properties
		private IList<Station> _stationList;
		public IList<Station> StationList
		{
			get { return _stationList; }
			set
			{
				_stationList = value;
				NotifyPropertyChanged();
			}
		}

		private Station _selectedStation;
		public Station SelectedStation
		{
			get { return _selectedStation; }
			set
			{
				if (_selectedStation == value)
				{
					return;
				}
				_selectedStation = value;
				NotifyPropertyChanged();
			}
		}

		private GameObject _targetGameObject;
		public GameObject TargetGameObject
		{
			get { return _targetGameObject; }
			set
			{
				if (_targetGameObject == value || value == null)
				{
					return;
				}
				_targetGameObject = value;

				CheckPrice(_targetGameObject);

				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<StationBuyOrderInfo> _buyOrdersInfo = new ObservableCollection<StationBuyOrderInfo>();
		public ObservableCollection<StationBuyOrderInfo> BuyOrdersInfo
		{
			get { return _buyOrdersInfo; }
			set
			{
				_buyOrdersInfo = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<StationSellOrderInfo> _sellOrdersInfo = new ObservableCollection<StationSellOrderInfo>();
		public ObservableCollection<StationSellOrderInfo> SellOrdersInfo
		{
			get { return _sellOrdersInfo; }
			set
			{
				_sellOrdersInfo = value;
				NotifyPropertyChanged();
			}
		}

		private RelayCommand _checkPriceCmd;
		public RelayCommand CheckPriceCmd
		{
			get
			{
				return _checkPriceCmd ?? (_checkPriceCmd = new RelayCommand(p => (TargetGameObject != null && TargetGameObject.TypeId != 0), p => CheckPrice(TargetGameObject)));
			}
		}

		private void CheckPrice(GameObject targetObject)
		{
			try
			{
				QuickLookResult orders = null;
				Task.Run(async () =>
				{
					orders = await Services.Instance.QuickLookAsync(targetObject.TypeId, new List<int>() {SelectedRegion.RegionId}, 1, SelectedSystem.SystemId);
				}).Wait();

				var sorted = orders.BuyOrders.OrderByDescending(t => t.Price);
				BuyOrdersInfo.Clear();
				foreach (var buyOrder in sorted)
				{
					BuyOrdersInfo.Add(new StationBuyOrderInfo()
					{
						StationName = buyOrder.StationName,
						BuyCount = buyOrder.VolumeRemaining,
						BuyPrice = buyOrder.Price
					});
				}

				sorted = orders.SellOrders.OrderBy(t => t.Price);
				SellOrdersInfo.Clear();
				foreach (var sellOrder in sorted)
				{
					SellOrdersInfo.Add(new StationSellOrderInfo()
					{
						StationName = sellOrder.StationName,
						SellCount = sellOrder.VolumeRemaining,
						SellPrice = sellOrder.Price
					});
				}
			}
			catch (Exception e)
			{
			}
		}
	}
}
