using System;
using System.Collections.Generic;
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

namespace PriceMonitor.UI.UiViewModels
{
	public class ItemTinyTradeHistoryViewModel : BaseViewModel
	{
		private readonly PlanetaryViewModel _planetaryViewModel;
		private readonly PITier _tier;

		public ItemTinyTradeHistoryViewModel(PlanetaryViewModel planetaryViewModel, PITier tier, GameObject gameObject, Station hub)
		{
			_planetaryViewModel = planetaryViewModel;
			_tier = tier;
			GameObject = gameObject;
			Hub = hub;
			_unicItemBrush = PickBrush();
			ExpanderBackgroundColor = _defaultBrush;

			CreateModel();
		}

		public void UpdatePiChain(bool build)
		{
			ExpanderBackgroundColor = build ? _unicItemBrush : _defaultBrush;

			_planetaryViewModel.PIObserving(new PlanetaryViewModel.PIObserveInfo()
			{
				PiID = GameObject.TypeId,
				Tier = _tier,
				ParentBrush = ExpanderBackgroundColor,
				CreatePiChain = build
			});
		}

		public void UpdatePiChain(PlanetaryViewModel.PIObserveInfo info)
		{
			ExpanderBackgroundColor = info.CreatePiChain ? info.ParentBrush : _defaultBrush;
		}

		public void ShowHistory(bool isVisible)
		{
			if (isVisible)
			{
				RequestHistory();
				UpdateTimeAxis((int)TimeFilter.TimeFilterEnum.Month);
			}
		}

		private readonly Brush _unicItemBrush;
		private readonly Brush _defaultBrush = new SolidColorBrush(Color.FromArgb(0xCC, 0x64, 0x76, 0x87));

		private Brush _expanderBackgroundColor;
		public Brush ExpanderBackgroundColor
		{
			get { return _expanderBackgroundColor; }
			set
			{
				_expanderBackgroundColor = value;
				NotifyPropertyChanged();
			}
		}

		private static readonly PropertyInfo[] Properties = typeof(Brushes).GetProperties();
		private Brush PickBrush()
		{
			Random rnd = new Random();

			int random = rnd.Next(Properties.Length);

			return (Brush)Properties[random].GetValue(null, null);
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

				var dataPoints =
					historyResponse.Items.Select(item => new DataPoint(DateTimeAxis.ToDouble(item.Date), item.HighPrice)).ToList();

				var hubChart = new LineSeries
				{
					Color = OxyColors.Blue
				};
				hubChart.Points.AddRange(dataPoints);

				Application.Current.Dispatcher.Invoke(() =>
				{
					Model.Series.Add(hubChart);
					UpdateTimeAxis((int)TimeFilter.TimeFilterEnum.Quarter);
				});
			}).ContinueWith(async t =>
			{
				var statResponse = await Services.Instance.MarketStatAsync(new List<int>() { GameObject.TypeId }, new List<int>() { Hub.RegionId });
				var k = statResponse.Count();
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