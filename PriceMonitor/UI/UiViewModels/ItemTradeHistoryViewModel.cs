using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

			RequestHistory();
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
						Title = hub.Name,
						Color = OxyColors.Automatic     //todo diff color
					};
					hubChart.Points.AddRange(dataPoints);

					Application.Current.Dispatcher.Invoke(() =>
					{
						Model.Series.Add(hubChart);
						Model.InvalidatePlot(true);
					});
				}
			}).ContinueWith(async t =>
			{
				table.Columns.Add("stats");

				var hubStats = new List<HubMarketStat>();
				foreach (var hub in Hubs)
				{
					table.Columns.Add(hub.Name);

					var statResponse = await Services.Instance.MarketStatAsync(new List<int>() { GameObject.TypeId }, new List<int>() { hub.RegionId });
					hubStats.Add(new HubMarketStat()
					{
						HubName = hub.Name,
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

			Model.Axes[1].FilterMinValue = DateTimeAxis.ToDouble(DateTime.Now - TimeSpan.FromDays(90));
		}
	}
}
