using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EveCentralProvider.Types
{
	[XmlRoot("type")]
	public class TypeMarketStats
	{
		/// <summary>
		/// The numeric ID for a EVE market type. e.g. 34 for Tritanium
		/// </summary>
		[XmlAttribute("id")]
		public int Id { get; set; }

		/// <summary>
		/// Market stats for buy orders for this type
		/// </summary>
		[XmlElement("buy", typeof(MarketStat))]
		public MarketStat Buy { get; set; }

		/// <summary>
		/// Market stats for sell orders for this type
		/// </summary>
		[XmlElement("sell", typeof(MarketStat))]
		public MarketStat Sell { get; set; }

		/// <summary>
		/// Market stats for all orders for this type
		/// </summary>
		[XmlElement("all", typeof(MarketStat))]
		public MarketStat All { get; set; }
	}

	[XmlRoot("marketstat")]
	public class MarketStatResult
	{
		[XmlElement("type")]
		public List<TypeMarketStats> type { get; set; }
	}

	[XmlRoot("evec_api")]
	public class EveCentralApiMarketStatResult
	{
		[XmlElement("marketstat")]
		public MarketStatResult marketstat { get; set; }
	}
}
