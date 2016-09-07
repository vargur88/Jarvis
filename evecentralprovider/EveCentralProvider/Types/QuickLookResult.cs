using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EveCentralProvider.Types
{
	[XmlRoot("quicklook")]
	public class QuickLookResult
	{
		[XmlElement("item")]
		public int Item { get; set; }

		[XmlElement("itemname")]
		public string ItemName { get; set; }

		[XmlElement("hours")]
		public int Hours { get; set; }

		[XmlElement("minqty")]
		public int MinimumQuantity { get; set; }

		[XmlArray("sell_orders"), XmlArrayItem("order")]
		public List<Order> SellOrders { get; set; }

		[XmlArray("buy_orders"), XmlArrayItem("order")]
		public List<Order> BuyOrders { get; set; }

		[XmlArray("regions"), XmlArrayItem("region")]
		public List<string> Regions { get; set; }
	}

	[XmlRoot("evec_api")]
	public class EveCentralApiQuickLookResult
	{
		public QuickLookResult quicklook { get; set; }
	}
}
