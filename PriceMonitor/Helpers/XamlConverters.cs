using PriceMonitor.UI.UiViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PriceMonitor.Helpers
{
	public class ItemFilterFlagToBoolConverter : IValueConverter
	{
		private ItemFilter target;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var flagToCheck = (ItemFilter) parameter;
			this.target = (ItemFilter)value;

			return this.target.HasFlag(flagToCheck);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var item = (ItemFilter) parameter;
			if (item == ItemFilter.All)
			{
				this.target = ItemFilter.All;
				return this.target;
			}

			this.target ^= (ItemFilter)parameter;
			return this.target;
		}
	}
}
