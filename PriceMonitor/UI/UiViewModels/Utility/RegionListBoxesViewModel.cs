using System.Collections.Generic;
using Entity.DataTypes;
using System.Linq;
using Entity;

namespace PriceMonitor.UI.UiViewModels
{
	public abstract class RegionListBoxesBaseViewModel : BaseViewModel
	{
		public virtual CommonMapObject FirstSelection { get; set; }
		public virtual CommonMapObject SecondSelection { get; set; }
		public virtual CommonMapObject ThirdSelection { get; set; }
	}

	public class RegionListBoxesViewModel<T> : RegionListBoxesBaseViewModel where T : RegionVisualizationType, new()
	{
		private readonly T _visualizationType;

		public RegionListBoxesViewModel()
		{
			_visualizationType = new T();

			FirstSelection = FirstList.First();
		}

		public IList<CommonMapObject> FirstList => _visualizationType.FirstList;

		private CommonMapObject _firstSelection;
		public override CommonMapObject FirstSelection
		{
			get => _firstSelection;
			set
			{
				if (_firstSelection == value || value == null)
				{
					return;
				}
				_firstSelection = value;
				NotifyPropertyChanged();

				SecondList = _visualizationType.FirstSelectionChanged(_firstSelection);

				if (SecondList.Any())
				{
					SecondSelection = SecondList.First();
				}
			}
		}

		public IList<CommonMapObject> SecondList
		{
			get => _visualizationType.SecondList;
			set => NotifyPropertyChanged();
		}

		private CommonMapObject _secondSelection;
		public override CommonMapObject SecondSelection
		{
			get => _secondSelection;
			set
			{
				if (_secondSelection == value || value == null)
				{
					return;
				}
				_secondSelection = value;
				NotifyPropertyChanged();

				ThirdList = _visualizationType.SecondSelectionChanged(_secondSelection);

				if (ThirdList.Any())
				{
					ThirdSelection = ThirdList.First();
				}
			}
		}

		public IList<CommonMapObject> ThirdList
		{
			get => _visualizationType.ThirdList;
			set => NotifyPropertyChanged();
		}

		private CommonMapObject _thirdSelection;
		public override CommonMapObject ThirdSelection
		{
			get => _thirdSelection;
			set
			{
				if (_thirdSelection == value)
				{
					return;
				}
				_thirdSelection = value;
				NotifyPropertyChanged();
			}
		}
	}

	public class RegionListBoxesViewModelAdapter : RegionListBoxesViewModel<FromRegionVisualizationType>
	{ }

	public class HubListBoxesViewModelAdapter : RegionListBoxesViewModel<FromHubVisualizationType>
	{ }

	public abstract class RegionVisualizationType
	{
		public abstract IList<CommonMapObject> FirstList { get; set; }
		public abstract IList<CommonMapObject> SecondList { get; set; }
		public abstract IList<CommonMapObject> ThirdList { get; set; }

		internal abstract IList<CommonMapObject> FirstSelectionChanged(CommonMapObject firstSelection);
		internal abstract IList<CommonMapObject> SecondSelectionChanged(CommonMapObject secondSelection);
	}

	public class FromRegionVisualizationType : RegionVisualizationType
	{
		public sealed override IList<CommonMapObject> FirstList { get; set; }
		public sealed override IList<CommonMapObject> SecondList { get; set; }
		public sealed override IList<CommonMapObject> ThirdList { get; set; }

		public FromRegionVisualizationType()
		{
			FirstList = EntityService.Instance.RequestRegionsAsync()
				.Result.Select(t => new CommonMapObject() { Id = t.RegionId, Name = t.Name }).ToList();
		}

		internal override IList<CommonMapObject> FirstSelectionChanged(CommonMapObject firstSelection)
		{
			SecondList = EntityService.Instance.RequestSystemsByRegionAsync((int)firstSelection.Id)
				.Result.Select(t => new CommonMapObject(){Id = t.SystemId, Name = t.Name}).ToList();

			return SecondList;
		}

		internal override IList<CommonMapObject> SecondSelectionChanged(CommonMapObject secondSelection)
		{
			ThirdList = EntityService.Instance.RequestStationsBySystemAsync((int)secondSelection.Id)
				.Result.Select(t => new CommonMapObject() { Id = t.StationId, Name = t.StationName }).ToList();

			return ThirdList;
		}
	}

	public class FromHubVisualizationType : RegionVisualizationType
	{
		public sealed override IList<CommonMapObject> FirstList { get; set; }
		public sealed override IList<CommonMapObject> SecondList { get; set; }
		public sealed override IList<CommonMapObject> ThirdList { get; set; }

		private readonly List<int> _regionHubList = new List<int>
		{
			10000002,	//Jita
			10000030,	//Rens
			10000032,	//Dodixie
			10000043,	//Amarr
			10000042,	//Hek
			10000047,	//Providence
			10000048,	//Orvolle
			10000014,	//GE-8JV
			10000030,	//Frarn
			10000002,	//Maila
		};

		private readonly List<int> _systemHubList = new List<int>
		{
			30000142,	//Jita
			30002510,	//Rens
			30002659,	//Dodixie
			30002187,	//Amarr
			30002053,	//Hek
			30003751,	//Providence
			30003830,	//Orvolle
			30001198,	//GE-8JV
			30002526,	//Frarn
			30000162,	//Maila
		};

		private readonly List<int> _stationHubList = new List<int>
		{
			60003760,	//Jita
			60004588,	//Rens
			60011866,	//Dodixie
			60008494,	//Amarr
			60004516,	//Hek
			61000221,	//Providence
			60011731,	//Orvolle
			61000182,	//GE-8JV
			60004615,	//Frarn
			60004081,	//Maila
		};

		public FromHubVisualizationType()
		{
			FirstList = EntityService.Instance.RequestRegionsAsync().Result
				.Where(t => _regionHubList.Contains(t.RegionId))
				.Select(t => new CommonMapObject() { Id = t.RegionId, Name = t.Name })
				.OrderBy(t => t.Id)
				.ToList();
		}

		internal override IList<CommonMapObject> FirstSelectionChanged(CommonMapObject firstSelection)
		{
			SecondList = EntityService.Instance.RequestSystemsByRegionAsync((int)firstSelection.Id).Result
				.Where(t => _systemHubList.Contains(t.SystemId))
				.Select(t => new CommonMapObject() { Id = t.SystemId, Name = t.Name })
				.ToList();

			return SecondList;
		}

		internal override IList<CommonMapObject> SecondSelectionChanged(CommonMapObject secondSelection)
		{
			ThirdList = EntityService.Instance.RequestStationsBySystemAsync((int)secondSelection.Id).Result
				.Where(t => _stationHubList.Contains((int)t.StationId))
				.Select(t => new CommonMapObject() { Id = t.StationId, Name = t.StationName })
				.ToList();

			return ThirdList;
		}
	}
}
