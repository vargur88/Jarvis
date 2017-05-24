using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Entity.DataTypes;
using EveCentralProvider;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PriceMonitor.DataTypes;
using PriceMonitor.Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class ItemTinyTradeHistoryViewModel : BaseViewModel
	{
		private readonly PlanetaryViewModel _planetaryViewModel;
		private readonly PITier _tier;

		public ItemTinyTradeHistoryViewModel(PlanetaryViewModel planetaryViewModel, PITier tier, Station hub, GameObject gameObject)
		{
			_planetaryViewModel = planetaryViewModel;
			_tier = tier;
			GameObject = gameObject;
			Hub = hub;
			_unicItemBrush = PickBrush();
			ExpanderBackgroundColor = BorderBrushColor = _defaultBrush;

			CreateModel();
		}

		public void UpdatePiChain(bool build)
		{
			if (build)
			{
				ExpanderBackgroundColor = _parentItemBrush ?? _unicItemBrush;
			}
			else
			{
				_parentItemBrush = null;
				ExpanderBackgroundColor = _defaultBrush;
			}

			_planetaryViewModel.PIObserving(new PlanetaryViewModel.PIObserveInfo()
			{
				PiID = GameObject.TypeId,
				Tier = _tier,
				ParentBrush = ExpanderBackgroundColor,
				CreatePiChain = build
			});
		}

		private readonly string MatchMark = " - Match";
		private bool Matching = false;
		public void UpdatePiChain(PlanetaryViewModel.PIObserveInfo info)
		{
			if (info.CreatePiChain)
			{
				// means that PI item was selected by another chain
				if (!Equals(ExpanderBackgroundColor, _defaultBrush))
				{
					GameObject.Name += MatchMark;
					Matching = true;

					NotifyPropertyChanged("GameObject");
				}
			}
			else
			{
				if (Matching)
				{
					GameObject.Name = GameObject.Name.Remove(GameObject.Name.Length - MatchMark.Length);
					Matching = false;

					NotifyPropertyChanged("GameObject");
				}
			}

			_parentItemBrush = info.ParentBrush;

			ExpanderBackgroundColor = info.CreatePiChain ? _parentItemBrush : _defaultBrush;
		}

		public void UpdateFocus(bool inFocus)
		{
			BorderBrushColor = inFocus ? _borderSelectedBrush : _defaultBrush;

			_planetaryViewModel.PIFocusing(new PlanetaryViewModel.PIObserveInfo()
			{
				PiID = GameObject.TypeId,
				Tier = _tier,
				InFocus = inFocus
			});
		}

		public void ChangeFocus(PlanetaryViewModel.PIObserveInfo info)
		{
			BorderBrushColor = info.InFocus ? _borderSelectedBrush : _defaultBrush;
		}

		public void ShowHistory(bool isVisible)
		{
			if (isVisible)
			{
				RequestHistory();
			}
		}

		private Brush _parentItemBrush;
		private readonly Brush _unicItemBrush;
		private readonly Brush _defaultBrush = new SolidColorBrush(Color.FromArgb(0xCC, 0x64, 0x76, 0x87));
		private readonly Brush _borderSelectedBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF));

		private Brush _expanderBackgroundColor;
		public Brush ExpanderBackgroundColor
		{
			get => _expanderBackgroundColor;
			set
			{
				_expanderBackgroundColor = value;
				NotifyPropertyChanged();
			}
		}

		private Brush _borderBrushColor;
		public Brush BorderBrushColor
		{
			get => _borderBrushColor;
			set
			{
				_borderBrushColor = value;
				NotifyPropertyChanged();
			}
		}

		private static readonly PropertyInfo[] Properties = typeof(Brushes).GetProperties();
		private static Random _random;
		private static object _syncObj = new object();
		private Brush PickBrush()
		{
			int rnd;
			lock (_syncObj)
			{
				if (_random == null)
				{
					_random = new Random();
				}
				rnd = _random.Next(Properties.Length);
			}

			return (Brush)Properties[rnd].GetValue(null, null);
		}

		private string _price;
		public string Price
		{
			get { return _price; }
			set
			{
				_price = value;
				NotifyPropertyChanged();
			}
		}

		private GameObject _gameObject;
		public GameObject GameObject
		{
			get { return _gameObject; }
			set
			{
				_gameObject = value;
				NotifyPropertyChanged();
			}
		}

		private Station Hub { get; set; }

		private DataTable _marketStatList;
		public DataTable MarketStatList
		{
			get { return _marketStatList; }
			set
			{
				_marketStatList = value;
				NotifyPropertyChanged();
			}
		}

		private PlotModel model;
		public PlotModel Model
		{
			get { return model; }
			set
			{
				if (model != value)
				{
					model = value;
					NotifyPropertyChanged();
				}
			}
		}

		private void UpdateTimeAxis(int days)
		{
			if (Model != null)
			{
				Model.Axes[1].FilterMinValue = days == 0 ? 0 : DateTimeAxis.ToDouble(DateTime.Now - TimeSpan.FromDays(days));

				Model.ResetAllAxes();
				Model.InvalidatePlot(true);
			}
		}

		private bool _historyRequested = false;

		private void RequestHistory()
		{
			if (_historyRequested)
			{
				return;
			}
			_historyRequested = true;

			Model.Series.Clear();

			Task.Run(async () =>
			{
				var historyResponse = await Services.Instance.HistoryAsync(GameObject.TypeId, Hub.RegionId);

				var dataPoints = historyResponse.Items
					.Where(t => DateTime.Now - t.Date <= TimeSpan.FromDays((int)TimeFilter.TimeFilterEnum.Month))
					.Select(t => new DataPoint(DateTimeAxis.ToDouble(t.Date), t.AvgPrice)).ToList();

				var hubChart = new LineSeries
				{
					Color = OxyColors.Blue
				};
				hubChart.Points.AddRange(dataPoints);

				Application.Current.Dispatcher.Invoke(() =>
				{
					Model.Series.Add(hubChart);

					int max = (int)(Model.Axes.First().Maximum = dataPoints.Max(t => t.Y));
					int min = (int)(Model.Axes.First().Minimum = dataPoints.Min(t => t.Y));

					var rawPriceStep = (max - min)/2;
					var stepDigitCount = (int)Math.Floor(Math.Log10(rawPriceStep) + 1);

					Model.Axes.First().MajorStep = (rawPriceStep).RoundOff(stepDigitCount - 1);
					UpdateTimeAxis((int)TimeFilter.TimeFilterEnum.Month);
				});
			}).ContinueWith(async t =>
			{
				/*var statResponse = await Services.Instance.MarketStatAsync(new List<int>() { GameObject.TypeId }, new List<int>() { Hub.RegionId });
				var k = statResponse.Count();*/
			});
		}

		private void CreateModel()
		{
			Model = new PlotModel
			{
				LegendBorder = OxyColors.Aqua,
				LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
				LegendPosition = LegendPosition.LeftBottom,
				LegendPlacement = LegendPlacement.Inside,
				Axes = {
				new LinearAxis()
				{
					Position = AxisPosition.Right,
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Solid,
					MajorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					MinorTicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					ExtraGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					AxislineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					TicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					MinorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					TextColor = OxyColors.Aqua
				},
				new DateTimeAxis()
				{
					Position = AxisPosition.Bottom,
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Solid,
					MajorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					MinorTicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					ExtraGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					AxislineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					TicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					MinorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					TextColor = OxyColors.Aqua
				}}
			};
		}
	}
}