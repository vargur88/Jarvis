using System;
using System.Collections.ObjectModel;
using System.Linq;
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
		public LookupStationListViewModel(ref ObservableCollection<Station> stationList)
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
					if (StationList.SingleOrDefault(k => k.StationId == StationBoxes.ThirdSelection.Id) == null)
					{
						StationList.Add(new Station()
						{
							RegionId = (int) StationBoxes.FirstSelection.Id,
							RegionName = StationBoxes.FirstSelection.Name,
							SystemId = (int) StationBoxes.SecondSelection.Id,
							SystemName = StationBoxes.SecondSelection.Name,
							StationId = (int) StationBoxes.ThirdSelection.Id,
							StationName = StationBoxes.ThirdSelection.Name
						});
					}
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

		private ObservableCollection<Station> _stationList;
		public ObservableCollection<Station> StationList
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
