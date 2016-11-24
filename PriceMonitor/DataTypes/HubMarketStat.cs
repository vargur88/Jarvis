using EveCentralProvider.Types;

namespace PriceMonitor.DataTypes
{
	public class HubMarketStat
	{
		public string HubName { get; set; }
		public MarketStat Stat { get; set; }
	}
}
