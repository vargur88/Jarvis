using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace EveCentralProvider.Types
{
	public class TypeHistory
	{
		public float Median { get; set; }

		public float Maximum { get; set; }

		public float Average { get; set; }

		public float StandardDeviation { get; set; }

		public float Minimum { get; set; }

		public long Volume { get; set; }

		public float FivePercent { get; set; }

		public DateTime At { get; set; }
	}

	public class TypeCrestHistory
	{
		public HistoryCrestItem[] Items { get; set; }

		public int PageCount { get; set; }

		public int TotalCount { get; set; }
	}

	public class HistoryCrestItem
	{
		public double OrderCount { get; set; }

		public double LowPrice { get; set; }

		public double HighPrice { get; set; }

		public double AvgPrice { get; set; }

		public long Volume { get; set; }

		public DateTime Date { get; set; }
	}
}
