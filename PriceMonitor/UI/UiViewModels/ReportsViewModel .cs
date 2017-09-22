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
using System.Windows.Input;
using PriceMonitor.Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class ReportsViewModel : BaseViewModel
	{
		public ReportsViewModel()
		{
			BuyHubCheck = true;
			SellHubCheck = false;
			FilterList = ItemFilter.All;

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

		private ItemFilter _filterList = ItemFilter.None;
		public ItemFilter FilterList
		{
			get => _filterList;
			set
			{
				_filterList = value;
				NotifyPropertyChanged();
			}
		}

		private RelayCommandBase<ItemFilter> _filterChangedCommand;
		public ICommand FilterChangedCommand => _filterChangedCommand ?? (_filterChangedCommand = new RelayCommandBase<ItemFilter>(UpdateFilterFlag));

		private void UpdateFilterFlag(ItemFilter item)
		{
			var func = PredicateBuilder.NewPredicate<eve_inv_types>();

			if (_filterList.HasFlag(ItemFilter.Tier1))
				func = func.And(t => t.meta_level < 5 && t.faction == "r");

			if (_filterList.HasFlag(ItemFilter.Tier2))
				func = func.And(t => t.meta_level == 5 && t.faction == "r");

			if (_filterList.HasFlag(ItemFilter.Faction))
				func = func.And(t => t.faction == "f" || (t.faction == "" && t.meta_level == 0));

			if (_filterList.HasFlag(ItemFilter.Deadsapce))
				func = func.And(t => t.faction == "d");

			if (_filterList.HasFlag(ItemFilter.Officer))
				func = func.And(t => t.faction == "o");

			if (_filterList == ItemFilter.All)
			{
				EntityService.Instance.FilterFunc = t => true;
				return;
			}

			EntityService.Instance.FilterFunc = func;
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

		private void CreateBasicsReportList(ObjectsNode obj, List<Func<BasicReportData>> tasks)
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

		/*
		 * For more intellectual report and/or master new API has to be introduced
		 * to prevent ban multiple objects info must be aggregate to single request for station/system
		 * as it recommended by API developers
		 */
		private Func<BasicReportData> CreateBasicReport(ObjectsNode obj)
		{
			return () =>
			{
				var report = new BasicReportData()
				{
					Item = new GameObject()
					{
						Name = obj.Object.Name,
						TypeId = obj.Object.TypeId
					},
					BuyStation = new Station()
					{
						RegionId = (int)BuyTarget.FirstSelection.Id,
						StationName = BuyTarget.ThirdSelection.Name
					},
					SellStation = new Station()
					{
						RegionId = (int)SellTarget.FirstSelection.Id,
						StationName = SellTarget.ThirdSelection.Name
					}
				};

				OrderCrest PriceConvert(OrderCrest order)
				{
					order.price = Math.Round(order.price / 1000000, 4);
					return order;
				}

				var resultSell = Services.Instance.ViewOrders(obj.Object.TypeId, (int)BuyTarget.FirstSelection.Id, sell: true);
				var resultBuy = Services.Instance.ViewOrders(obj.Object.TypeId, (int)BuyTarget.FirstSelection.Id, sell: false);

				if (resultSell?.items != null && resultSell.items.Count > 0)
				{
					report.BuyStationSellOrders = resultSell.items.OrderBy(k => k.price).Take(5).Select(PriceConvert).ToList();
				}
				if (resultBuy?.items != null && resultBuy.items.Count > 0)
				{
					report.BuyStationBuyOrders = resultBuy.items.OrderByDescending(k => k.price).Take(5).Select(PriceConvert).ToList();
				}

				resultSell = Services.Instance.ViewOrders(obj.Object.TypeId, (int)SellTarget.FirstSelection.Id, sell: true);
				resultBuy = Services.Instance.ViewOrders(obj.Object.TypeId, (int)SellTarget.FirstSelection.Id, sell: false);

				if (resultSell?.items != null && resultSell.items.Count > 0)
				{
					report.SellStationSellOrders = resultSell.items.OrderBy(k => k.price).Take(5).Select(PriceConvert).ToList();
				}
				if (resultBuy?.items != null && resultBuy.items.Count > 0)
				{
					report.SellStationBuyOrders = resultBuy.items.OrderByDescending(k => k.price).Take(5).Select(PriceConvert).ToList();
				}

				var diffSell = report.SellStationSellOrders.First().price == 0
					? report.BuyStationSellOrders.First().price
					: (report.SellStationSellOrders.First().price - report.BuyStationSellOrders.First().price);

				var diffBuy = report.SellStationSellOrders.First().price == 0
					? report.BuyStationBuyOrders.First().price
					: (report.SellStationSellOrders.First().price - report.BuyStationBuyOrders.First().price);

				var instantProffit = report.SellStationSellOrders.First().price == 0
					? 0xFFFFFF
					: report.SellStationBuyOrders.First().price - report.BuyStationSellOrders.First().price;

				var instStr = (instantProffit == 0xFFFFFF) ? "up to you" : Math.Round(instantProffit, 4).ToString();
				report.Proffit = $"{Math.Round(diffSell, 4)}/{Math.Round(diffBuy, 4)}/{instStr}";

				return report;
			};
		}

		private void GenerateReport()
		{
			var tasks = new List<Func<BasicReportData>>();
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
				Parallel.ForEach(tasks, task =>
				{
					Action<BasicReportData> nextItem;

					while (!bag.TryTake(out nextItem)) { }

					nextItem.Invoke(task.Invoke());
				});
			});
		}
	}

	public class BasicReportData
	{
		public GameObject Item { get; set; }
		public Station BuyStation { get; set; }
		public Station SellStation { get; set; }
		public string Proffit { get; set; }

		public List<OrderCrest> BuyStationSellOrders { get; set; } = new List<OrderCrest>() {new OrderCrest()};
		public List<OrderCrest> BuyStationBuyOrders { get; set; } = new List<OrderCrest>() {new OrderCrest()};
		public List<OrderCrest> SellStationSellOrders { get; set; } = new List<OrderCrest>() {new OrderCrest()};
		public List<OrderCrest> SellStationBuyOrders { get; set; } = new List<OrderCrest>() {new OrderCrest()};
	}

	[FlagsAttribute]
	public enum ItemFilter
	{
		None = 0,
		Tier1 = 1 << 0,
		Tier2 = 1 << 1,
		Faction = 1 << 2,
		Deadsapce = 1 << 3,
		Officer = 1 << 4,
		All = Tier1 | Tier2 | Faction | Deadsapce | Officer
	}
}
