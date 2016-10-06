using System;
using Entity.DataTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Helpers;
using PriceMonitor.UI.UiViewModels;
using System.Windows;
using EveCentralProvider;
using EveCentralProvider.Types;

namespace PriceMonitor
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			Task.Run(() =>
			{
				var regionList = EntityService.Instance.RequestRegionsAsync();

				Application.Current.Dispatcher.Invoke(() =>
				{
					foreach (var item in EntityService.Instance.RequestObjectNodes())
					{
						MenuItems.Add(item);
					}

					Charts.Add(new ChartViewModel(regionList.Result));
					Charts.Add(new ChartViewModel(regionList.Result));

					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
					Reports.Add(new BasicReportViewModel());
				});
			});
		}

		private int menuCount;
		public int MenuCount
		{
			get { return menuCount; }
			set
			{
				menuCount = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<ChartViewModel> _charts = new ObservableCollection<ChartViewModel>();
		public ObservableCollection<ChartViewModel> Charts
		{
			get { return _charts; }
			set
			{
				_charts = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<BasicReportViewModel> _reports = new ObservableCollection<BasicReportViewModel>();
		public ObservableCollection<BasicReportViewModel> Reports
		{
			get { return _reports; }
			set
			{
				_reports = value;
				NotifyPropertyChanged();
			}
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

		private ObjectsNode _selectedNode;
		public ObjectsNode SelectedNode
		{
			get { return _selectedNode; }
			set
			{
				_selectedNode = value;

				if (_selectedNode.Object.TypeId != 0)
				{
					foreach (var chart in Charts)
					{
						chart.TargetGameObject = _selectedNode.Object;
					}
				}

				NotifyPropertyChanged();
			}
		}

		private RelayCommand _generateReportCmd;
		public RelayCommand GenerateReportCmd
		{
			get
			{
				return _generateReportCmd ?? (_generateReportCmd = new RelayCommand(p => (SelectedNode != null && _selectedNode.Object.TypeId == 0), p => GenerateReport(SelectedNode)));
			}
		}

		private void GenerateReport(ObjectsNode nodeToCheck)
		{
			int count = 0;

			var firstStation = Charts.First().SelectedStation;
			var secondStation = Charts.Last().SelectedStation;

			foreach (var obj in nodeToCheck.SubObjects)
			{
				if (count == 5)
				{
					break;
				}

				if (obj.Object.TypeId == 0)
				{
					continue;
				}

				QuickLookResult firstStationOrders;
				QuickLookResult secondStationOrders;
				Task.Run(async () =>
				{
					firstStationOrders = await Services.Instance.QuickLookAsync(obj.Object.TypeId, new List<int>() { firstStation.RegionId }, 1, firstStation.SystemId);
					secondStationOrders = await Services.Instance.QuickLookAsync(obj.Object.TypeId, new List<int>() { secondStation.RegionId }, 1, secondStation.SystemId);

					// TODO have to be included in final report
					if (firstStationOrders.SellOrders.Count == 0 || secondStationOrders.SellOrders.Count == 0)
					{
						return;
					}

					var sortedFirst = firstStationOrders.SellOrders.OrderBy(t => t.Price).ToList();
					var sortedSecond = secondStationOrders.SellOrders.OrderBy(t => t.Price).ToList();

					var whereToBuy = sortedFirst.First().Price < sortedSecond.First().Price ? sortedFirst : sortedSecond;

					var prices = new List<float> { whereToBuy.First().Price };
					float firstBuyPrice = prices.First();
					long buyVolume = whereToBuy.First().VolumeRemaining;

					foreach (var order in whereToBuy.Where(order => order.Price - firstBuyPrice <= firstBuyPrice * 0.05))
					{
						prices.Add(order.Price);
						buyVolume += order.VolumeRemaining;
					}

					long averagePrice = prices.Sum(price => (long)price) / prices.Count;

					++count;
				}).Wait();
			}
		}
	}
}
