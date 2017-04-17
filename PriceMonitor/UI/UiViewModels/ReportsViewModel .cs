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
using System.Threading;
using System.Collections.Concurrent;

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
			get => _menuItems;
			set
			{
				_menuItems = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<BasicReportViewModel> _basicReportsItems = new ObservableCollection<BasicReportViewModel>();
		public ObservableCollection<BasicReportViewModel> BasicReportsItems
		{
			get => _basicReportsItems;
			set
			{
				//if (!Equals(_basicReportsItems, value))
				{
					_basicReportsItems = value;
					NotifyPropertyChanged();
				}
			}
		}

		private ObjectsNode _selectedNode;
		public ObjectsNode SelectedNode
		{
			get => _selectedNode;
			set
			{
				_selectedNode = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Region> _regionListFirst = new List<Region>();
		public IList<Region> RegionListFirst
		{
			get => _regionListFirst;
			set
			{
				_regionListFirst = value;
				NotifyPropertyChanged();
			}
		}

		private IList<SolarSystem> _systemListFirst = new List<SolarSystem>();
		public IList<SolarSystem> SystemListFirst
		{
			get => _systemListFirst;
			set
			{
				_systemListFirst = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Station> _stationListFirst = new List<Station>();
		public IList<Station> StationListFirst
		{
			get => _stationListFirst;
			set
			{
				_stationListFirst = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Region> _regionListSecond = new List<Region>();
		public IList<Region> RegionListSecond
		{
			get => _regionListSecond;
			set
			{
				_regionListSecond = value;
				NotifyPropertyChanged();
			}
		}

		private IList<SolarSystem> _systemListSecond = new List<SolarSystem>();
		public IList<SolarSystem> SystemListSecond
		{
			get => _systemListSecond;
			set
			{
				_systemListSecond = value;
				NotifyPropertyChanged();
			}
		}

		private IList<Station> _stationListSecond = new List<Station>();
		public IList<Station> StationListSecond
		{
			get => _stationListSecond;
			set
			{
				_stationListSecond = value;
				NotifyPropertyChanged();
			}
		}

		private Region _selectedRegionFirst;
		public Region SelectedRegionFirst
		{
			get => _selectedRegionFirst;
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
			get => _selectedSystemFirst;
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
			get => _selectedStationFirst;
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
			get => _selectedRegionSecond;
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
			get => _selectedSystemSecond;
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
			get => _selectedStationSecond;
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

		private void CreateBasicsReportList(ObjectsNode obj, List<Action<Action<BasicReportData>>> tasks)
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

		private Action<Action<BasicReportData>> CreateBasicReport(ObjectsNode obj)
		{
			return ((callback) =>
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

				// TODO merge sell/buy from one station to single structure. Maybe only prices
				var result = Services.Instance.QuickLook(obj.Object.TypeId, new List<int>() { SelectedStationFirst.RegionId }, 1, SelectedStationFirst.SystemId);
				if (result.SellOrders != null && result.SellOrders.Any())
				{
					report.BuyStationSellOrders = result.SellOrders.OrderBy(k => k.Price).Take(5).Select(PriceConvert).ToList();
				}
				if (result.BuyOrders != null && result.BuyOrders.Any())
				{
					report.BuyStationBuyOrders = result.BuyOrders.OrderByDescending(k => k.Price).Take(5).Select(PriceConvert).ToList();
				}

				result = Services.Instance.QuickLook(obj.Object.TypeId, new List<int>() { SelectedStationSecond.RegionId }, 1, SelectedStationSecond.SystemId);
				if (result.SellOrders != null && result.SellOrders.Any())
				{
					report.SellStationSellOrders = result.SellOrders.OrderBy(k => k.Price).Take(5).Select(PriceConvert).ToList();
				}
				if (result.BuyOrders != null && result.BuyOrders.Any())
				{
					report.SellStationBuyOrders = result.BuyOrders.OrderByDescending(k => k.Price).Take(5).Select(PriceConvert).ToList();
				}

				var diffSell = report.SellStationSellOrders.First().Price == 0
					? report.BuyStationSellOrders.First().Price
					: (report.SellStationSellOrders.First().Price - report.BuyStationSellOrders.First().Price);

				var diffBuy = report.SellStationSellOrders.First().Price == 0
					? report.BuyStationBuyOrders.First().Price
					: (report.SellStationSellOrders.First().Price - report.BuyStationBuyOrders.First().Price);

				var instantProffit = report.SellStationBuyOrders.First().Price - report.BuyStationSellOrders.First().Price;

				report.Proffit = $"{Math.Round(diffSell, 4)}/{Math.Round(diffBuy, 4)}/{Math.Round(instantProffit, 4)}";

				callback(report);
			});
		}

		private void GenerateReport()
		{
			var tasks = new List<Action<Action<BasicReportData>>>();
			CreateBasicsReportList(SelectedNode, tasks);

			var bag = new ConcurrentBag<Action<BasicReportData>>();
			for (var i = 0; i < tasks.Count; ++i)
			{
				var view = new BasicReportViewModel();

				bag.Add(view.AssignReport);
				BasicReportsItems.Add(view);
			}

			Parallel.ForEach(tasks, t =>
			{
				Action<BasicReportData> nextItem;

				while (!bag.TryTake(out nextItem)){}

				t.Invoke(nextItem);
			});
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
