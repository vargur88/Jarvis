using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EveCentralProvider.Types
{
	[XmlRoot("minerals")]
	public class EveMonResult
	{
		[XmlElement("mineral")]
		public List<Mineral> Minerals { get; set; }
	}

	[XmlRoot("mineral")]
	public class Mineral
	{
		[XmlElement]
		public string name { get; set; }

		[XmlElement]
		public float price { get; set; }
	}
}
