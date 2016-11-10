using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EveCentralProvider;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace PriceMonitor.UI.UiViewModels
{
	public class ItemTradeHistoryViewModel : BaseViewModel
	{
		public ItemTradeHistoryViewModel()
		{
			Model = CreateModel(1);
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

		private PlotModel CreateModel(int count)
		{
			var newModel = new PlotModel
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
					TextColor = OxyColors.Aqua,
					FormatAsFractions = true
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

			Task.Run(async () =>
			{
				var s = new LineSeries { Title = "Jita" };
				s.Color = OxyColors.Gray;
				newModel.Series.Add(s);
				var k = await Services.Instance.HistoryAsync(2865, 10000002);

				foreach (var item in k.Items)
				{
					s.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), item.HighPrice));
				}
			}).Wait();

			Task.Run(async () =>
			{
				var s = new LineSeries { Title = "Amarr" };
				s.Color = OxyColors.Blue;
				newModel.Series.Add(s);
				var k = await Services.Instance.HistoryAsync(2865, 10000043);

				foreach (var item in k.Items)
				{
					s.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), item.HighPrice));
				}
			}).Wait();

			Task.Run(async () =>
			{
				var s = new LineSeries { Title = "Hek" };
				s.Color = OxyColors.Green;
				newModel.Series.Add(s);
				var k = await Services.Instance.HistoryAsync(2865, 10000042);

				foreach (var item in k.Items)
				{
					s.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), item.HighPrice));
				}
			}).Wait();

			newModel.Axes[1].FilterMinValue = DateTimeAxis.ToDouble(DateTime.Now - TimeSpan.FromDays(90));

			return newModel;
		}

	}
}
