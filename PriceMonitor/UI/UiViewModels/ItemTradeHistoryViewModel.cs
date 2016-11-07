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
			Model = CreateModel(3);
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
				LegendBorder = OxyColors.Black,
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
					MinorTicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a)
				},
				new LinearAxis()
				{
					Position = AxisPosition.Bottom,
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Solid,
					MajorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
					MinorTicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a)
				}}
			};

			for (var i = 1; i <= count; i++)
			{
				var s = new LineSeries { Title = "Series " + i };
				newModel.Series.Add(s);
				for (double x = 0; x < 2*Math.PI; x += 0.1)
				{
					s.Points.Add(new DataPoint(x, Math.Sin(x*i)/i + i));
				}
			}
			return newModel;
		}

	}
}
