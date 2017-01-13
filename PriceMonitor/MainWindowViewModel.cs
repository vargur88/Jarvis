using Entity.DataTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Helpers;
using PriceMonitor.UI.UiViewModels;
using System.Windows;
using EveCentralProvider;

namespace PriceMonitor
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			//WatchingVM = new WatchingViewModel();
			ReportsVM = new ReportsViewModel();

			Task.Run(() =>
			{
				//var regionList = EntityService.Instance.RequestRegionsAsync();

				Application.Current.Dispatcher.Invoke(() =>
				{

					//Charts.Add(new ChartViewModel(regionList.Result));
					//Charts.Add(new ChartViewModel(regionList.Result));
				});
			});
		}

		public WatchingViewModel WatchingVM { get; private set; }

		public ReportsViewModel ReportsVM { get; private set; }

		private int _menuCount;
		public int MenuCount
		{
			get { return _menuCount; }
			set
			{
				_menuCount = value;
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
			var firstStation = Charts.First().SelectedStation;
			var secondStation = Charts.Last().SelectedStation;

			Task.Run(async () =>
			{
				foreach (var obj in nodeToCheck.SubObjects.Where(obj => obj.Object.TypeId != 0))
				{
					var firstStationOrders = await Services.Instance.QuickLookAsync(obj.Object.TypeId, new List<int>() {firstStation.RegionId}, 1, firstStation.SystemId)
						.ContinueWith(t =>
						{
							return t.Result.SellOrders.OrderBy(k => k.Price);
						});

					/*var tst = await Services.Instance.MarketStatAsync(
						new List<int>() {obj.Object.TypeId},
						new List<int>() {firstStation.RegionId},
						1,
						firstStation.SystemId);*/

					var secondStationOrders = await Services.Instance.QuickLookAsync(obj.Object.TypeId, new List<int>() {secondStation.RegionId}, 1, secondStation.SystemId)
						.ContinueWith(t =>
						{
							return t.Result.SellOrders.OrderBy(k => k.Price);
						});

					var whereToBuy = firstStationOrders.First().Price < secondStationOrders.First().Price ?
						firstStationOrders : secondStationOrders;

					var prices = new List<float> {whereToBuy.First().Price};
					float firstBuyPrice = prices.First();
					long buyVolume = whereToBuy.First().VolumeRemaining;

					foreach (var order in whereToBuy.Where(order => order.Price - firstBuyPrice <= firstBuyPrice*0.05))
					{
						prices.Add(order.Price);
						buyVolume += order.VolumeRemaining;
					}

					long averagePrice = prices.Sum(price => (long) price)/prices.Count;
				}
			}).Wait();
	}
	}
}
