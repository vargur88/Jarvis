using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Entity.DataTypes;
using EveCentralProvider;
using EveCentralProvider.Types;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PriceMonitor.DataTypes;

namespace PriceMonitor.UI.UiViewModels
{
	public class ItemTradeHistoryViewModel : BaseViewModel
	{
		public ItemTradeHistoryViewModel(GameObject gameObject, List<Station> hubs)
		{
			GameObject = gameObject;
			Hubs = hubs;

			CreateModel();

			//RequestHistory();
		}

		private GameObject GameObject { get; set; }
		private List<Station> Hubs { get; set; }

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

		public CollectionView TimeFilters { get; } = new CollectionView(new List<TimeFilter>()
		{
			new TimeFilter(TimeFilter.TimeFilterEnum.Day),
			new TimeFilter(TimeFilter.TimeFilterEnum.Week),
			new TimeFilter(TimeFilter.TimeFilterEnum.Month),
			new TimeFilter(TimeFilter.TimeFilterEnum.Quarter),
			new TimeFilter(TimeFilter.TimeFilterEnum.Year),
			new TimeFilter(TimeFilter.TimeFilterEnum.AllTime),
		});

		private TimeFilter _selectedTimeFilter = new TimeFilter(TimeFilter.TimeFilterEnum.Month);
		public TimeFilter SelectedTimeFilter
		{
			get { return _selectedTimeFilter; }
			set
			{
				if (_selectedTimeFilter == value)
				{
					return;
				}

				_selectedTimeFilter = value;
				NotifyPropertyChanged();

				UpdateTimeAxis((int)SelectedTimeFilter.Value);
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

		private readonly Dictionary<string, OxyColor> _hubColors = new Dictionary<string, OxyColor>()
		{
			{"Jita", OxyColors.Blue},
			{"Amarr", OxyColors.Red },
			{"Hek", OxyColors.Gray }
		};

		private OxyColor ResolveColorFromHub(string hubName)
		{
			OxyColor color;
			if (_hubColors.TryGetValue(hubName, out color))
			{
				return color;
			}

			return OxyColors.Automatic;
		}

		private void RequestHistory()
		{
			Model.Series.Clear();

			DataTable table = new DataTable();

			Task.Run(async () =>
			{
				foreach (var hub in Hubs)
				{
					var historyResponse = await Services.Instance.HistoryAsync(GameObject.TypeId, hub.RegionId);

					var dataPoints =
						historyResponse.Items.Select(item => new DataPoint(DateTimeAxis.ToDouble(item.Date), item.HighPrice)).ToList();

					var hubChart = new LineSeries
					{
						Title = hub.StationName,
						Color = ResolveColorFromHub(hub.StationName)
					};
					hubChart.Points.AddRange(dataPoints);

					Application.Current.Dispatcher.Invoke(() =>
					{
						Model.Series.Add(hubChart);
						UpdateTimeAxis((int)SelectedTimeFilter.Value);
					});
				}
			}).ContinueWith(async t =>
			{
				table.Columns.Add("stats");

				var hubStats = new List<HubMarketStat>();
				foreach (var hub in Hubs)
				{
					table.Columns.Add(hub.StationName);

					var statResponse = await Services.Instance.MarketStatAsync(new List<int>() { GameObject.TypeId }, new List<int>() { hub.RegionId });
					hubStats.Add(new HubMarketStat()
					{
						HubName = hub.StationName,
						Stat = statResponse.First().Sell
					});
				}

				var stats = typeof(MarketStat).GetProperties();
				foreach (var stat in stats)
				{
					var nextRow = table.NewRow();

					int index = 0;
					nextRow[index] = stat.Name;

					foreach (var hubStat in hubStats)
					{
						index++;
						nextRow[index] = typeof(MarketStat).GetProperty(stat.Name).GetValue(hubStat.Stat, null);
					}
					table.Rows.Add(nextRow);
				}

				Application.Current.Dispatcher.Invoke(() =>
				{
					MarketStatList = table;
				});
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

	public class TimeFilter
	{
		public TimeFilter(TimeFilterEnum value)
		{
			Value = value;
			Name = Value.ToString();
		}

		public string Name { get; set; }
		public TimeFilterEnum Value { get; set; }

		public enum TimeFilterEnum
		{
			Day = 2,
			Week = 7,
			Month = 30,
			Quarter = 120,
			Year = 365,
			AllTime = 0
		}
	}
}
