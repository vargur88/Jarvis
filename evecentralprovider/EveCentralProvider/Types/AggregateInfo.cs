using Newtonsoft.Json;

namespace EveCentralProvider.Types
{
	public class AggregateInfoList
	{
		public AggregateInfo[] Items { get; set; }

		public string RegionId { get; set; }
	}

	public class AggregateInfo
	{
		public string id { get; set; }

		public AggreateInfoStat buy { get; set; }

		public AggreateInfoStat sell { get; set; }
	}

	public class AggreateInfoStat
	{
		[JsonIgnore]
		public string RegionName { get; set; }

		public float weightedAverage { get; set; }

		public float max { get; set; }

		public float min { get; set; }

		public float stddev { get; set; }

		public float median { get; set; }

		public float volume { get; set; }

		public int orderCount { get; set; }

		public float percentile { get; set; }
	}
}
