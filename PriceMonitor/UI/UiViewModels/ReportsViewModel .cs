using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Entity;
using Entity.DataTypes;
using Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class ReportsViewModel : BaseViewModel
	{
		public ReportsViewModel()
		{
			var hubs = new List<Station>()
			{
				new Station()
				{
					Name = "Jita",
					SystemId = 10000002,
					RegionId = 10000002
				},
				new Station()
				{
					Name = "Amarr",
					SystemId = 10000043,
					RegionId = 10000043
				},
				new Station()
				{
					Name = "Hek",
					SystemId = 10000042,
					RegionId = 10000042
				}
			};

			Application.Current.Dispatcher.Invoke(() =>
			{
				foreach (var item in EntityService.Instance.RequestObjectNodes())
				{
					MenuItems.Add(item);
				}

				//Charts.Add(new ChartViewModel(regionList.Result));
				//Charts.Add(new ChartViewModel(regionList.Result));
			});

			BasicReportsItems.Add(new BasicReportViewModel());
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

		private RelayCommand _generateReportCmd;
		public RelayCommand GenerateReportCmd
		{
			get
			{
				return _generateReportCmd ?? (_generateReportCmd = 
					new RelayCommand(
						p => SelectedNode != null && SelectedNode.SubObjects == null, 
						p => Foo()/*CheckPrice(SelectedChain, FirstRegion, SecondRegion)*/));
			}
		}

		private void Foo()
		{}
	}
}
