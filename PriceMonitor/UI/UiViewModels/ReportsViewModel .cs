using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Entity;
using Entity.DataTypes;
using EveCentralProvider;
using EveCentralProvider.Types;
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
					SelectedRegionSecond = RegionListSecond.Single(t => t.RegionId == 10000042); // The Metropolis

					SystemListFirst = EntityService.Instance.RequestSystemsByRegionAsync(SelectedRegionFirst.RegionId).Result;
					SystemListSecond = EntityService.Instance.RequestSystemsByRegionAsync(SelectedRegionSecond.RegionId).Result;
					SelectedSystemFirst = SystemListFirst.Single(t => t.SystemId == 30000142); // Jita
					SelectedSystemSecond = SystemListSecond.Single(t => t.SystemId == 30002053); // Hek

					StationListFirst = EntityService.Instance.RequestStationsBySystemAsync(SelectedSystemFirst.SystemId).Result;
					StationListSecond = EntityService.Instance.RequestStationsBySystemAsync(SelectedSystemSecond.SystemId).Result;
					SelectedStationFirst = StationListFirst.Single(t => t.StationId == 60003760); //Jita IV - Moon 4 - Caldari Navy Assembly Plant
					SelectedStationSecond = StationListSecond.Single(t => t.StationId == 60004516); //Hek IV - Krusual Tribe Bureau

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
						p => SelectedNode != null, 
						p => GenerateReport()));
			}
		}

		private void CreateBasicsReportList(ObjectsNode obj, List<Task<BasicReportData>> tasks)
		{
			if (obj.SubObjects == null)
			{
				tasks.Add(CreateBasicReport(obj));
				return;
			}

			foreach (var item in obj.SubObjects)
			{
				if (item.SubObjects != null)
				{
					CreateBasicsReportList(item, tasks);
				}
				else
				{
					tasks.Add(CreateBasicReport(item));
				}
			}
		}

		private Task<BasicReportData> CreateBasicReport(ObjectsNode obj)
		{
			return Task.Factory.StartNew(() =>
			{
				/*var firstStat = await Services.Instance.MarketStatAsync(
					new List<int>() { obj.Object.TypeId },
					new List<int>() { SelectedStationFirst.RegionId },
					1,
					SelectedStationFirst.SystemId);

				var second = await Services.Instance.MarketStatAsync(
					new List<int>() { obj.Object.TypeId },
					new List<int>() { SelectedStationFirst.RegionId },
					1,
					SelectedStationFirst.SystemId);*/

				/*
				 * average calculation
				 * var prices = new List<float> { whereToBuy.First().Price };
				float firstBuyPrice = prices.First();
				long buyVolume = whereToBuy.First().VolumeRemaining;

				foreach (var order in whereToBuy.Where(order => order.Price - firstBuyPrice <= firstBuyPrice * 0.02))
				{
					prices.Add(order.Price);
					buyVolume += order.VolumeRemaining;
				}

				long averagePrice = prices.Sum(price => (long)price) / prices.Count;*/

				var report = new BasicReportData()
				{
					ItemName = obj.Object.Name,
					BuyStation = SelectedStationFirst.Name,
					SellStation = SelectedStationSecond.Name
				};

			    Order PriceConvert(Order order)
			    {
			        order.Price = Math.Round(order.Price / 1000000, 4);
			        return order;
			    }

			    var task1 = Services.Instance.QuickLookAsync(obj.Object.TypeId, new List<int>() {SelectedStationFirst.RegionId}, 1, SelectedStationFirst.SystemId)
					.ContinueWith(t =>
					{
						if (t.Result.SellOrders != null && t.Result.SellOrders.Any())
						{
							report.BuyStationSellOrders = t.Result.SellOrders.OrderBy(k => k.Price).Take(5).Select(PriceConvert).ToList();
						}

						if (t.Result.BuyOrders != null && t.Result.BuyOrders.Any())
						{
						    report.BuyStationBuyOrders = t.Result.BuyOrders.OrderByDescending(k => k.Price).Take(5).Select(PriceConvert).ToList();
						}
					});

				var task2 = Services.Instance.QuickLookAsync(obj.Object.TypeId, new List<int>() {SelectedStationSecond.RegionId}, 1, SelectedStationSecond.SystemId)
					.ContinueWith(t =>
					{
						if (t.Result.SellOrders != null && t.Result.SellOrders.Any())
						{
						    report.SellStationSellOrders = t.Result.SellOrders.OrderBy(k => k.Price).Take(5).Select(PriceConvert).ToList();
						}

						if (t.Result.BuyOrders != null && t.Result.BuyOrders.Any())
						{
						    report.SellStationBuyOrders = t.Result.BuyOrders.OrderByDescending(k => k.Price).Take(5).Select(PriceConvert).ToList();
						}
					});

				Task.WaitAll(task1, task2);

				var diffSell = report.SellStationSellOrders.First().Price == 0
					? report.BuyStationSellOrders.First().Price
					: (report.SellStationSellOrders.First().Price - report.BuyStationSellOrders.First().Price);

				var diffBuy = report.SellStationSellOrders.First().Price == 0
					? report.BuyStationBuyOrders.First().Price
					: (report.SellStationSellOrders.First().Price - report.BuyStationBuyOrders.First().Price);

				var instantProffit = report.SellStationBuyOrders.First().Price - report.BuyStationSellOrders.First().Price;

				report.Proffit = $"{Math.Round(diffSell, 4)}/{Math.Round(diffBuy, 4)}/{Math.Round(instantProffit, 4)}";

				return report;
			});
		}

		private void GenerateReport()
		{
			var tasks = new List<Task<BasicReportData>>();
			CreateBasicsReportList(SelectedNode, tasks);

			BasicReportsItems.Clear();
			foreach (var task in tasks)
			{
				BasicReportsItems.Add(new BasicReportViewModel(task));
			}
		}
	}

	public class BasicReportData
	{
		public string ItemName { get; set; }
		public string BuyStation { get; set; }
		public string SellStation { get; set; }
		public string Proffit { get; set; }

		public List<Order> BuyStationSellOrders { get; set; } = new List<Order>() {new Order()};
		public List<Order> BuyStationBuyOrders { get; set; } = new List<Order>() {new Order()};
		public List<Order> SellStationSellOrders { get; set; } = new List<Order>() {new Order()};
		public List<Order> SellStationBuyOrders { get; set; } = new List<Order>() {new Order()};
	}
}
