using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EveCentralProvider.Types
{
	[XmlRoot("order")]
	public class Order
	{
		[XmlAttribute("id")]
		public long Id { get; set; }

		[XmlElement("region")]
		public int Region { get; set; }

		[XmlElement("station")]
		public int Station { get; set; }

		[XmlElement("station_name")]
		public string StationName { get; set; }

		[XmlElement("security")]
		public float Security { get; set; }

		[XmlElement("range")]
		public int Range { get; set; }

		[XmlElement("price")]
		public float Price { get; set; }

		[XmlElement("vol_remain")]
		public long VolumeRemaining { get; set; }

		[XmlElement("min_volume")]
		public long MinimumVolume { get; set; }

		[XmlElement("expires")]
		public DateTime Expires { get; set; }

		// I haven't decided on the best place to parse this field to a DateTime since the API's output is... nonstandard.
		[XmlElement("reported_time")]
		public string ReportedTime { get; set; }
	}
}
