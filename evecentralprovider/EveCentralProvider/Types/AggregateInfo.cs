namespace EveCentralProvider.Types
{
	public class AggregateInfoList
	{
		public AggregateInfo[] ItemsInfos { get; set; }
	}

	public class AggregateInfo
	{
		public int ItemId { get; set; }

		public AggreateInfoStat Buy { get; set; }

		public AggreateInfoStat Sell { get; set; }
	}

	public class AggreateInfoStat
	{
		public double weightedAverage { get; set; }

		public float max { get; set; }

		public float min { get; set; }

		public double stddev { get; set; }

		public float median { get; set; }

		public float volume { get; set; }

		public int orderCount { get; set; }

		public float percentile { get; set; }
	}
}
