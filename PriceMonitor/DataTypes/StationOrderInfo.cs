namespace PriceMonitor.DataTypes
{
	public class StationBuyOrderInfo
	{
		public string StationName { get; set; }
		public float BuyPrice { get; set; }
		public long BuyCount { get; set; }
	}

	public class StationSellOrderInfo
	{
		public string StationName { get; set; }
		public float SellPrice { get; set; }
		public long SellCount { get; set; }
	}
}
