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

	public class StationHubToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
				return false;

			string checkValue = value.ToString();
			string targetValue = parameter.ToString();

			return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
				return null;

			bool useValue = (bool)value;
			string targetValue = parameter.ToString();

			if (useValue)
			{
				return Enum.Parse(targetType, targetValue);
			}

			return null;
		}
	}
}
