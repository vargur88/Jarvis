using System;
using System.Collections.ObjectModel;
using Entity.DataTypes;
using Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public enum HubStationType
	{
		None, Hub, Default, Custom
	}

	public class LookupStationListViewModel : BaseViewModel
	{
		public LookupStationListViewModel(ref ObservableCollection<CommonMapObject> stationList)
		{
			SelectedStationType = HubStationType.Hub;
			_stationList = stationList;
		}

		public Action CloseAction { get; set; }

		private RelayCommand _addStationCmd;
		public RelayCommand AddStationCmd
		{
			get
			{
				return _addStationCmd ?? (_addStationCmd = new RelayCommand(p => StationBoxes.ThirdSelection != null, t =>
				{
					StationList.Add(StationBoxes.ThirdSelection);
				}));
			}
		}

		private RelayCommand _closeCmd;
		public RelayCommand CloseCmd
		{
			get
			{
				return _closeCmd ?? (_closeCmd = new RelayCommand(t =>
				{
				}));
			}
		}

		private RegionListBoxesBaseViewModel _stationBoxes;
		public RegionListBoxesBaseViewModel StationBoxes
		{
			get => _stationBoxes;
			set
			{
				_stationBoxes = value;
				NotifyPropertyChanged();
			}
		}

		private HubStationType _selectedStationType;
		public HubStationType SelectedStationType
		{
			get => _selectedStationType;
			set
			{
				if (_selectedStationType == value)
				{
					return;
				}

				_selectedStationType = value;

				switch (_selectedStationType)
				{
					case HubStationType.Hub:
						StationBoxes = new RegionListBoxesViewModel<FromHubVisualizationType>();
						break;
					case HubStationType.Default:
						StationBoxes = new RegionListBoxesViewModel<FromRegionVisualizationType>();
						break;
					case HubStationType.Custom:
						StationBoxes = new RegionListBoxesViewModel<FromHubVisualizationType>();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<CommonMapObject> _stationList = new ObservableCollection<CommonMapObject>();
		public ObservableCollection<CommonMapObject> StationList
		{
			get => _stationList;
			set
			{
				_stationList = value;
				NotifyPropertyChanged();
			}
		}

		public void CancelAction()
		{
			_stationList.Clear();
		}
	}
}
