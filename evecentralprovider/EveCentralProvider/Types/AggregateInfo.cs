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

		public double weightedAverage { get; set; }

		public double max { get; set; }

		public double min { get; set; }

		public double stddev { get; set; }

		public double median { get; set; }

		public double volume { get; set; }

		public int orderCount { get; set; }

		public double percentile { get; set; }
	}
}
