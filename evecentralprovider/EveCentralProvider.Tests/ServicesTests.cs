using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using EveCentralProvider.Types;
using System.Linq;
using System.Threading.Tasks;

namespace EveCentralProvider.Tests
{
	[TestClass]
	public class ServicesTests
	{
		private static List<int> RegionId = new List<int>() { 10000014 };
		private static List<int> TypeId = new List<int>() { 34, 35, 36 };

		[TestMethod]
		public void MarketStatTest()
		{
			Services target = Services.Instance;
			List<TypeMarketStats> output = target.MarketStat(TypeId, RegionId);

			Assert.IsTrue(output.Count == 3);
			Assert.IsTrue(output[0].Id == 34);
			Assert.IsTrue(output[1].Id == 35);
			Assert.IsTrue(output[2].Id == 36);

		}

		[TestMethod]
		public void MarketStatAsyncTest()
		{
			Services target = Services.Instance;
			List<TypeMarketStats> output = target.MarketStatAsync(TypeId, RegionId).Result;

			Assert.IsTrue(output.Count == 3);
			Assert.IsTrue(output[0].Id == 34);
			Assert.IsTrue(output[1].Id == 35);
			Assert.IsTrue(output[2].Id == 36);
		}

		[TestMethod]
		public void QuickLookTest()
		{
			Services target = Services.Instance;
			QuickLookResult output = target.QuickLook(TypeId[0], RegionId);

			Assert.IsTrue(output.Item == 34);
			Assert.IsTrue(output.ItemName == "Tritanium");
			Assert.IsTrue(output.BuyOrders.Count > 0);
			Assert.IsTrue(output.SellOrders.Count > 0);

		}

		[TestMethod]
		public void QuickLookAsyncTest()
		{
			Services target = Services.Instance;
			QuickLookResult output = target.QuickLookAsync(TypeId[0], RegionId).Result;

			Assert.IsTrue(output.Item == 34);
			Assert.IsTrue(output.ItemName == "Tritanium");
			Assert.IsTrue(output.BuyOrders.Count > 0);
			Assert.IsTrue(output.SellOrders.Count > 0);

		}

		[TestMethod]
		public void QuickLookPathTest()
		{
			Services target = Services.Instance;
			QuickLookPathResult output = target.QuickLookPath("Jita", "Amarr", 34);

			Assert.IsTrue(output.Item == 34);
			Assert.IsTrue(output.ItemName == "Tritanium");
			Assert.IsTrue(output.BuyOrders.Count > 0);
			Assert.IsTrue(output.SellOrders.Count > 0);
			Assert.IsTrue(output.From == 30000142);
			Assert.IsTrue(output.To == 30002187);
		}

		[TestMethod]
		public void QuickLookPathAsyncTest()
		{
			Services target = Services.Instance;
			QuickLookPathResult output = target.QuickLookPathAsync("Jita", "Amarr", 34).Result;

			Assert.IsTrue(output.Item == 34);
			Assert.IsTrue(output.ItemName == "Tritanium");
			Assert.IsTrue(output.BuyOrders.Count > 0);
			Assert.IsTrue(output.SellOrders.Count > 0);
			Assert.IsTrue(output.From == 30000142);
			Assert.IsTrue(output.To == 30002187);
		}

		[TestMethod]
		public void HistoryTest()
		{
			Services target = Services.Instance;
			List<TypeHistory> output = target.History(34, LocaleType.System, "Amarr", OrderType.Sell);

			Assert.IsTrue(output.Count > 0);
			Assert.IsTrue(output[0].Volume > 0);
		}

		[TestMethod]
		public void HistoryAsyncTest()
		{
			Services target = Services.Instance;
			List<TypeHistory> output = target.HistoryAsync(34, LocaleType.System, "Amarr", OrderType.Sell).Result;

			Assert.IsTrue(output.Count > 0);
			Assert.IsTrue(output[0].Volume > 0);
		}

		[TestMethod]
		public void EveMonTest()
		{
			Services target = Services.Instance;
			EveMonResult output = target.EveMon();

			Assert.IsTrue(output.Minerals.All(mineral => mineral.name != null && mineral.name != "" && mineral.price != 0));
		}

		[TestMethod]
		public void EveMonAsyncTest()
		{
			Services target = Services.Instance;
			EveMonResult output = target.EveMonAsync().Result;

			Assert.IsTrue(output.Minerals.All(mineral => mineral.name != null && mineral.name != "" && mineral.price != 0));
		}

		[TestMethod]
		public void RouteTest()
		{
			Services target = Services.Instance;
			List<RouteJump> output = target.Route("Jita", "HED-GP");

			Assert.AreEqual("Jita", output[0].From.Name);
			Assert.AreEqual("Perimeter", output[0].To.Name);

			Assert.AreEqual("Perimeter", output[1].From.Name);
		}

		[TestMethod]
		public void RouteAsyncTest()
		{
			Services target = Services.Instance;
			List<RouteJump> output = target.RouteAsync("Jita", "HED-GP").Result;

			Assert.AreEqual("Jita", output[0].From.Name);
			Assert.AreEqual("Perimeter", output[0].To.Name);

			Assert.AreEqual("Perimeter", output[1].From.Name);
		}

	}
}
