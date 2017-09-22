using System;
using System.Collections.Generic;

namespace EveCentralProvider.Types
{
	public enum OrderType
	{
		Sell = 0,
		Buy = 1
	}

	public class OrderCrestList
	{
		public string totalCount_str { get; set; }
		public List<OrderCrest> items { get; set; }
		public int pageCount { get; set; }
		public string pageCount_str { get; set; }
		public int totalCount { get; set; }
	}

	public class OrderCrest
	{
		public string volume_str { get; set; }
		public bool buy { get; set; }
		public DateTime issued { get; set; }
		public double price { get; set; }
		public long volumeEntered { get; set; }
		public int minVolume { get; set; }
		public long volume { get; set; }
		public string range { get; set; }
		public string href { get; set; }
		public string duration_str { get; set; }
		public OrderLocation location { get; set; }
	}

	public class OrderLocation
	{
		public string id_str { get; set; }
		public string href { get; set; }
		public long id { get; set; }
		public string name { get; set; }
	}
}
