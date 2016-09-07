using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EveCentralProvider.Types
{
	[DataContract]
	public class RouteJump
	{
		[DataMember]
		public RoutePoint From { get; set; }

		[DataMember]
		public RoutePoint To { get; set; }
	}

	[DataContract]
	public class RoutePoint
	{
		[DataMember]
		public int SystemId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public float Security { get; set; }

		[DataMember]
		public RouteRegion Region { get; set; }

		[DataMember]
		public int ConstellationId { get; set; }
	}

	[DataContract]
	public class RouteRegion
	{
		[DataMember]
		public int RegionId { get; set; }

		[DataMember]
		public string Name { get; set; }

	}
}
