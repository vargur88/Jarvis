﻿using System;
namespace PriceMonitor.Helpers
{
	public static class Extensions
	{
		public static T Next<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException(string.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

			T[] arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf<T>(arr, src) + 1;
			return (arr.Length == j) ? arr[0] : arr[j];
		}

		public static int RoundOff(this int i, int nearest)
		{
			nearest = (int)Math.Pow(10, nearest);
			return (i + 5 * nearest / 10) / nearest * nearest;
		}
	}
}
