using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Threading.Tasks;
using System.Windows;
using Entity;
using Entity.DataTypes;
using EveCentralProvider;
using Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class ReportsViewModel : BaseViewModel
	{
		public ReportsViewModel()
		{
			Task.Run(() =>
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					// REWORK DAT SHIT
					RegionListFirst = RegionListSecond = EntityService.Instance.RequestRegionsAsync().Result;
					SelectedRegionFirst = RegionListFirst.Single(t => t.RegionId == 10000002); // The Forge
					SelectedRegionSecond = RegionListSecond.Single(t => t.RegionId == 10000002); // The Forge

					SystemListFirst = SystemListSecond = EntityService.Instance.RequestSystemsByRegionAsync(SelectedRegionFirst.RegionId).Result;
					SelectedSystemFirst = SystemListFirst.Single(t => t.SystemId == 30000142); // Jita
					SelectedSystemSecond = SystemListSecond.Single(t => t.SystemId == 30000142); // Jita

					StationListFirst = StationListSecond = EntityService.Instance.RequestStationsBySystemAsync(SelectedSystemFirst.SystemId).Result;
					SelectedStationFirst = StationListFirst.Single(t => t.StationId == 60003760); //Jita IV - Moon 4 - Caldari Navy Assembly Plant
					SelectedStationSecond = StationListSecond.Single(t => t.StationId == 60003760); //Jita IV - Moon 4 - Caldari Navy Assembly Plant

					foreach (var item in EntityService.Instance.RequestObjectNodes())
					{
						MenuItems.Add(item);
					}
				});
			});
		}

		private ObservableCollection<ObjectsNode> _menuItems = new ObservableCollection<ObjectsNode>();
		public ObservableCollection<ObjectsNode> MenuItems
		{
			get { return _menuItems; }
			set
			{
				_menuItems = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<BasicReportViewModel> _basicReportsItems = new ObservableCollection<BasicReportViewModel>();
		public ObservableCollection<BasicReportViewModel> BasicReportsItems
		{
			get { return _basicReportsItems; }
			set
			{
				_basicReportsItems = value;
				NotifyPropertyChanged();
			}
		}

		private ObjectsNode _selectedNode;
		public ObjectsNode SelectedNode
		{
			get { return _selectedNode; }
			set
			{
				_selectedNode = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Region> _regionListFirst = new List<Region>();
		public IList<Region> RegionListFirst
		{
			get { return _regionListFirst; }
			set
			{
				_regionListFirst = value;
				NotifyPropertyChanged();
			}
		}

		private IList<SolarSystem> _systemListFirst = new List<SolarSystem>();
		public IList<SolarSystem> SystemListFirst
		{
			get { return _systemListFirst; }
			set
			{
				_systemListFirst = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Station> _stationListFirst = new List<Station>();
		public IList<Station> StationListFirst
		{
			get { return _stationListFirst; }
			set
			{
				_stationListFirst = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Region> _regionListSecond = new List<Region>();
		public IList<Region> RegionListSecond
		{
			get { return _regionListSecond; }
			set
			{
				_regionListSecond = value;
				NotifyPropertyChanged();
			}
		}

		private IList<SolarSystem> _systemListSecond = new List<SolarSystem>();
		public IList<SolarSystem> SystemListSecond
		{
			get { return _systemListSecond; }
			set
			{
				_systemListSecond = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Station> _stationListSecond = new List<Station>();
		public IList<Station> StationListSecond
		{
			get { return _stationListSecond; }
			set
			{
				_stationListSecond = value;
				NotifyPropertyChanged();
			}
		}

		private Region _selectedRegionFirst;
		public Region SelectedRegionFirst
		{
			get { return _selectedRegionFirst; }
			set
			{
				if (_selectedRegionFirst == value || value == null)
				{
					return;
				}
				_selectedRegionFirst = value;
				NotifyPropertyChanged();

				SystemListFirst = EntityService.Instance.RequestSystemsByRegionAsync(value.RegionId).Result;
				if (SystemListFirst != null && SystemListFirst.Count > 0)
				{
					SelectedSystemFirst = SystemListFirst.First();
				}
			}
		}

		private SolarSystem _selectedSystemFirst;
		public SolarSystem SelectedSystemFirst
		{
			get { return _selectedSystemFirst; }
			set
			{
				if (_selectedSystemFirst == value || value == null)
				{
					return;
				}
				_selectedSystemFirst = value;
				NotifyPropertyChanged();

				StationListFirst = EntityService.Instance.RequestStationsBySystemAsync(value.SystemId).Result;
				if (StationListFirst != null && StationListFirst.Count > 0)
				{
					SelectedStationFirst = StationListFirst.First();
				}
			}
		}

		private Station _selectedStationFirst;
		public Station SelectedStationFirst
		{
			get { return _selectedStationFirst; }
			set
			{
				if (_selectedStationFirst == value)
				{
					return;
				}
				_selectedStationFirst = value;
				NotifyPropertyChanged();
			}
		}

		private Region _selectedRegionSecond;
		public Region SelectedRegionSecond
		{
			get { return _selectedRegionSecond; }
			set
			{
				if (_selectedRegionSecond == value || value == null)
				{
					return;
				}
				_selectedRegionSecond = value;
				NotifyPropertyChanged();

				SystemListSecond = EntityService.Instance.RequestSystemsByRegionAsync(value.RegionId).Result;
				if (SystemListSecond != null && SystemListSecond.Count > 0)
				{
					SelectedSystemSecond = SystemListSecond.First();
				}
			}
		}

		private SolarSystem _selectedSystemSecond;
		public SolarSystem SelectedSystemSecond
		{
			get { return _selectedSystemSecond; }
			set
			{
				if (_selectedSystemSecond == value || value == null)
				{
					return;
				}
				_selectedSystemSecond = value;
				NotifyPropertyChanged();

				StationListSecond = EntityService.Instance.RequestStationsBySystemAsync(value.SystemId).Result;
				if (StationListSecond != null && StationListSecond.Count > 0)
				{
					SelectedStationSecond = StationListSecond.First();
				}
			}
		}

		private Station _selectedStationSecond;
		public Station SelectedStationSecond
		{
			get { return _selectedStationSecond; }
			set
			{
				if (_selectedStationSecond == value)
				{
					return;
				}
				_selectedStationSecond = value;
				NotifyPropertyChanged();
			}
		}

		private RelayCommand _generateReportCmd;
		public RelayCommand GenerateReportCmd
		{
			get
			{
				return _generateReportCmd ?? (_generateReportCmd = 
					new RelayCommand(
						p => SelectedNode?.SubObjects != null, 
						p => GenerateReport()));
			}
		}

		private void CreateNextReport(IEnumerable<ObjectsNode> items, List<Task<BasicReportData>> tasks)
		{
			foreach (var item in items)
			{
				if (item.SubObjects != null)
				{
					CreateNextReport(item.SubObjects, tasks);
				}
				else
				{
					tasks.Add(Task.Factory.StartNew(() =>
					{
						/*var tst = await Services.Instance.MarketStatAsync(
							new List<int>() { SelectedNode.Object.TypeId },
							new List<int>() { SelectedStationFirst.RegionId },
							1,
							SelectedStationFirst.SystemId);*/

						var firstStationOrders = Services.Instance.QuickLook(item.Object.TypeId, new List<int>() { SelectedStationFirst.RegionId }, 1, SelectedStationFirst.SystemId)
							.SellOrders.OrderBy(k => k.Price);

						var secondStationOrders = Services.Instance.QuickLook(item.Object.TypeId, new List<int>() { SelectedStationSecond.RegionId }, 1, SelectedStationSecond.SystemId)
							.SellOrders.OrderBy(k => k.Price);

						var whereToBuy = firstStationOrders.First().Price < secondStationOrders.First().Price ?
							firstStationOrders : secondStationOrders;

						var prices = new List<float> { whereToBuy.First().Price };
						float firstBuyPrice = prices.First();
						long buyVolume = whereToBuy.First().VolumeRemaining;

						foreach (var order in whereToBuy.Where(order => order.Price - firstBuyPrice <= firstBuyPrice * 0.02))
						{
							prices.Add(order.Price);
							buyVolume += order.VolumeRemaining;
						}

						long averagePrice = prices.Sum(price => (long)price) / prices.Count;

						return new BasicReportData()
						{
							ItemName = item.Object.Name,
							BuyStation = whereToBuy.First().StationName,
							SellStation = (whereToBuy.First().StationName == firstStationOrders.First().StationName) 
								? secondStationOrders.First().StationName
								: firstStationOrders.First().StationName
						};
					}));
				}
			}
		}

		private void GenerateReport()
		{
			var tasks = new List<Task<BasicReportData>>();
			CreateNextReport(SelectedNode.SubObjects, tasks);

			BasicReportsItems.Clear();
			foreach (var task in tasks)
			{
				BasicReportsItems.Add(new BasicReportViewModel(task));
			}
		}
	}

	public struct BasicReportData
	{
		public string ItemName { get; set; }
		public string BuyStation { get; set; }
		public string SellStation { get; set; }
	}
}
