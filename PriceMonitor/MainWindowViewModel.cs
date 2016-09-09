using System;
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
using EveCentralProvider.Types;

namespace PriceMonitor
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			Task.Run(() =>
			{
				var regionList = EntityService.Instance.RequestRegionsAsync().Result;

				Application.Current.Dispatcher.Invoke(() =>
				{
					Charts.Add(new ChartViewModel(regionList));
					Charts.Add(new ChartViewModel(regionList));

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

				MenuItems = EntityService.Instance.RequestChainAsync().Result;
			});
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

				if (_selectedChain.Object.TypeId != 0)
				{
					foreach (var chart in Charts)
					{
						chart.TargetGameObject = _selectedChain.Object;
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
				return _generateReportCmd ?? (_generateReportCmd = new RelayCommand(p => (SelectedChain != null && _selectedChain.Object.TypeId == 0), p => GenerateReport(SelectedChain)));
			}
		}

		private void GenerateReport(ObjectsChain chainToCheck)
		{
			int count = 0;

			var firstStation = Charts.First().SelectedStation;
			var secondStation = Charts.Last().SelectedStation;

			foreach (var obj in chainToCheck.SubObjects)
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

					var prices = new List<float> {whereToBuy.First().Price};
					float firstBuyPrice = prices.First();
					long buyVolume = whereToBuy.First().VolumeRemaining;

					foreach (var order in whereToBuy.Where(order => order.Price - firstBuyPrice <= firstBuyPrice*0.05))
					{
						prices.Add(order.Price);
						buyVolume += order.VolumeRemaining;
					}

					long averagePrice = prices.Sum(price => (long) price) / prices.Count;

					++count;
				}).Wait();
			}
		}
	}
}
