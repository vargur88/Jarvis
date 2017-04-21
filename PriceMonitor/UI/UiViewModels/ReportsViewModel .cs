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
using System.Collections.Concurrent;

namespace PriceMonitor.UI.UiViewModels
{
	public class ReportsViewModel : BaseViewModel
	{
		public ReportsViewModel()
		{
			BuyHubCheck = true;
			SellHubCheck = false;

			Task.Run(() =>
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					foreach (var item in EntityService.Instance.RequestObjectNodes())
					{
						MenuItems.Add(item);
					}
				});
			});
		}

		private bool _buyHubCheck;
		public bool BuyHubCheck
		{
			get => _buyHubCheck;
			set
			{
				_buyHubCheck = value;
				NotifyPropertyChanged();

				if (_buyHubCheck)
				{
					BuyTarget = new RegionListBoxesViewModel<FromHubVisualizationType>();
				}
				else
				{
					BuyTarget = new RegionListBoxesViewModel<FromRegionVisualizationType>();
				}
			}
		}

		private bool _sellHubCheck;
		public bool SellHubCheck
		{
			get => _sellHubCheck;
			set
			{
				_sellHubCheck = value;
				NotifyPropertyChanged();

				if (_sellHubCheck)
				{
					SellTarget = new RegionListBoxesViewModel<FromHubVisualizationType>();
				}
				else
				{
					SellTarget = new RegionListBoxesViewModel<FromRegionVisualizationType>();
				}
			}
		}

		private RegionListBoxesBaseViewModel _buyTarget;
		public RegionListBoxesBaseViewModel BuyTarget
		{
			get => _buyTarget;
			set
			{
				_buyTarget = value;
				NotifyPropertyChanged();
			}
		}

		private RegionListBoxesBaseViewModel _sellTarget;
		public RegionListBoxesBaseViewModel SellTarget
		{
			get => _sellTarget;
			set
			{
				_sellTarget = value;
				NotifyPropertyChanged();
			}
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
				_basicReportsItems = value;
				NotifyPropertyChanged();
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
					BuyStation = BuyTarget.ThirdSelection.Name,
					SellStation = SellTarget.ThirdSelection.Name
				};

				Order PriceConvert(Order order)
				{
					order.Price = Math.Round(order.Price / 1000000, 4);
					return order;
				}

				// TODO merge sell/buy from one station to single structure. Maybe only prices
				var result = Services.Instance.QuickLook(obj.Object.TypeId, new List<int>() { (int)BuyTarget.FirstSelection.Id }, 1, (int)BuyTarget.SecondSelection.Id);
				if (result.SellOrders != null && result.SellOrders.Any())
				{
					report.BuyStationSellOrders = result.SellOrders.OrderBy(k => k.Price).Take(5).Select(PriceConvert).ToList();
				}
				if (result.BuyOrders != null && result.BuyOrders.Any())
				{
					report.BuyStationBuyOrders = result.BuyOrders.OrderByDescending(k => k.Price).Take(5).Select(PriceConvert).ToList();
				}

				result = Services.Instance.QuickLook(obj.Object.TypeId, new List<int>() { (int)SellTarget.FirstSelection.Id }, 1, (int)SellTarget.SecondSelection.Id);
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
			BasicReportsItems.Clear();
			for (var i = 0; i < tasks.Count; ++i)
			{
				var view = new BasicReportViewModel();

				bag.Add(view.AssignReport);
				BasicReportsItems.Add(view);
			}

			Task.Factory.StartNew(() =>
			{
				Parallel.ForEach(tasks, t =>
				{
					Action<BasicReportData> nextItem;

					while (!bag.TryTake(out nextItem)) { }

					t.Invoke(nextItem);
				});
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
