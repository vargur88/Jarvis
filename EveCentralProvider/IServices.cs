using EveCentralProvider.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCentralProvider
{
	public interface IServices
	{
		/// <summary>
		/// Retrieve aggregate statistics for the items specified.
		/// </summary>
		/// <param name="typeid">The type ID of the item you are requesting. I.e., 34 for Tritanium. Can be specified more than once</param>
		/// <param name="minQ">The minimum quantity in an order to consider it for the statistics</param>
		/// <param name="regionlimit">Restrict statistics to a region. Can be specified more than once.</param>
		/// <param name="usesystem">Restrict statistics to a system.</param>
		/// <param name="hours">Statistics from the last X specified hours. Defaults to 24.</param>
		/// <returns></returns>
		List<TypeMarketStats> MarketStat(List<int> typeid, List<int> regionlimit, int minQ = 10001, int usesystem = 0, int hours = 24);
		Task<List<TypeMarketStats>> MarketStatAsync(List<int> typeid, List<int> regionlimit, int minQ = 10001, int usesystem = 0, int hours = 24);

		/// <summary>
		/// Retrieve all of the available market orders, including prices, stations, order IDs, volumes, etc.
		/// </summary>
		/// <param name="typeid">The type ID to be queried</param>
		/// <param name="setminQ">Restrict the view to show only orders above the specified quantity</param>
		/// <param name="regionlimit">Restrict the view to only show from within the specified region IDs. Can be specified multiple times.</param>
		/// <param name="usesystem">Restrict the view to the following system only.</param>
		/// <param name="sethours">Get orders which have been posted within the last X hours. Defaults to 360</param>
		/// <returns></returns>
		QuickLookResult QuickLook(int typeid, List<int> regionlimit, int setminQ = 10001, int usesystem = 0, int sethours = 360);
		Task<QuickLookResult> QuickLookAsync(int typeid, List<int> regionlimit, int setminQ = 10001, int usesystem = 0, int sethours = 360);

		/// <summary>
		/// Retrieve all of the available market orders, including prices, stations, order IDs, volumes, etc., on a given jump path.
		/// </summary>
		/// <param name="start">The name or solarsystem ID where to start the path</param>
		/// <param name="end">The name or solarsystem ID where to end the path</param>
		/// <param name="type">The numeric type ID for which to show (i.e., 34 for Tritanium)</param>
		/// <param name="setminQ">Restrict the view to show only orders above the specified quantity</param>
		/// <param name="sethours">Get orders which have been posted within the last X hours. Defaults to 360</param>
		/// <returns></returns>
		QuickLookPathResult QuickLookPath(string start, string end, int type, int setminQ, int sethours = 360);
		Task<QuickLookPathResult> QuickLookPathAsync(string start, string end, int type, int setminQ, int sethours = 360);

		/// <summary>
		/// You can get a snapshot of EVE-Central statistics (not game statistics) over time.
		/// </summary>
		/// <param name="type">The type ID (numeric only) of the item you wish to get statistics for.</param>
		/// <param name="bid">Bid = 1 for buy orders, 0 for sell orders</param>
		/// <param name="locale">The literal text “system” or “region”, meaning to look at per system or per region statistics.</param>
		/// <param name="idOrName">If you specified “system” or “region”, the text or numerical ID of the system or region.</param>
		/// <returns></returns>
		List<TypeHistory> History(int type, LocaleType locale, string idOrName, OrderType bid);
		Task<List<TypeHistory>> HistoryAsync(int type, LocaleType locale, string idOrName, OrderType bid);

		/// <summary>
		/// Retrieve the median mineral prices in the empire regions in a format digestible by EVEmon.
		/// </summary>
		/// <returns></returns>
		EveMonResult EveMon();
		Task<EveMonResult> EveMonAsync();

		/// <summary>
		/// Retrieve a shortest-path route between two places
		/// </summary>
		/// <param name="start">The name or solarsystem ID where to start the path</param>
		/// <param name="end">The name or solarsystem ID where to end the path</param>
		/// <returns></returns>
		List<RouteJump> Route(string start, string end);
		Task<List<RouteJump>> RouteAsync(string start, string end);
	}
}
