using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EveCentralProvider.Types
{
	[XmlRoot]
	public class MarketStat
	{
		[XmlElement(ElementName = "volume")]
		public long Volume { get; set; }

		[XmlElement(ElementName = "avg")]
		public float Average { get; set; }

		[XmlElement(ElementName = "max")]
		public float Maximum { get; set; }

		[XmlElement(ElementName = "min")]
		public float Minimum { get; set; }

		[XmlElement(ElementName = "stddev")]
		public float StandardDeviation { get; set; }

		[XmlElement(ElementName = "median")]
		public float Median { get; set; }

		[XmlElement(ElementName = "percentile")]
		public float Percentile { get; set; }
	}
}
