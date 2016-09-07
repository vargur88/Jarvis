using EveCentralProvider.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EveCentralProvider
{
	public sealed class Services : IServices
	{
		private readonly string ApiFormat = "http://api.eve-central.com/api/{0}?{1}";
		private readonly string UserAgent = String.Format(".NET Eve Central Provider v{0}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);

		private static readonly Services _instance = new Services();
		public static Services Instance { get { return _instance; } }

		private Services()
		{

		}

		public List<TypeMarketStats> MarketStat(List<int> typeid, List<int> regionlimit, int minQ = 10001, int usesystem = 0, int hours = 24)
		{
			Uri apiUrl = BuildMarketStatUrl(typeid, regionlimit, minQ, usesystem, hours);

			Stream stream;
			if (apiUrl.ToString().Length > 2000)
			{
				stream = Post(apiUrl);
			}
			else
			{
				stream = Get(apiUrl);
			}

			XmlSerializer xml = new XmlSerializer(typeof(EveCentralApiMarketStatResult));
			var results = (EveCentralApiMarketStatResult)xml.Deserialize(stream);
			return results.marketstat.type;
		}

		private Uri BuildMarketStatUrl(List<int> typeid, List<int> regionlimit, int minQ, int usesystem, int hours)
		{
			var args = HttpUtility.ParseQueryString(string.Empty);

			foreach (int type in typeid)
			{
				args.Add("typeid", type.ToString());
			}

			args.Add("minQ", minQ.ToString());

			foreach (int region in regionlimit)
			{
				args.Add("regionlimit", region.ToString());
			}

			if (usesystem != 0)
				args.Add("usesystem", usesystem.ToString());

			if (hours != 24)
				args.Add("hours", hours.ToString());

			Uri apiUrl = new Uri(String.Format(ApiFormat, "marketstat", args.ToString()));
			return apiUrl;
		}

		public async Task<List<TypeMarketStats>> MarketStatAsync(List<int> typeid, List<int> regionlimit, int minQ = 10001, int usesystem = 0, int hours = 24)
		{
			Uri apiUrl = BuildMarketStatUrl(typeid, regionlimit, minQ, usesystem, hours);

			Stream stream;
			if (apiUrl.ToString().Length > 2000)
			{
				stream = await PostAsync(apiUrl);
			}
			else
			{
				stream = await GetAsync(apiUrl);
			}

			XmlSerializer xml = new XmlSerializer(typeof(EveCentralApiMarketStatResult));
			var results = (EveCentralApiMarketStatResult)xml.Deserialize(stream);
			return results.marketstat.type;
		}

		public QuickLookResult QuickLook(int typeid, List<int> regionlimit, int setminQ = 10001, int usesystem = 0, int sethours = 360)
		{
			Uri apiUrl = BuildQuickLookUrl(typeid, regionlimit, setminQ, usesystem, sethours);

			Stream stream;
			if (apiUrl.ToString().Length > 2000)
			{
				stream = Post(apiUrl);
			}
			else
			{
				stream = Get(apiUrl);
			}

			XmlSerializer xml = new XmlSerializer(typeof(EveCentralApiQuickLookResult));
			var results = (EveCentralApiQuickLookResult)xml.Deserialize(stream);
			return results.quicklook;
		}

		private Uri BuildQuickLookUrl(int typeid, List<int> regionlimit, int setminQ, int usesystem, int sethours)
		{
			var args = HttpUtility.ParseQueryString(string.Empty);

			args.Add("typeid", typeid.ToString());

			foreach (int region in regionlimit)
			{
				args.Add("regionlimit", region.ToString());
			}

			args.Add("setminQ", setminQ.ToString());

			if (usesystem != 0)
				args.Add("usesystem", usesystem.ToString());

			if (sethours != 360)
				args.Add("sethours", sethours.ToString());

			Uri apiUrl = new Uri(String.Format(ApiFormat, "quicklook", args.ToString()));
			return apiUrl;
		}

		public async Task<QuickLookResult> QuickLookAsync(int typeid, List<int> regionlimit, int setminQ = 10001, int usesystem = 0, int sethours = 360)
		{
			Uri apiUrl = BuildQuickLookUrl(typeid, regionlimit, setminQ, usesystem, sethours);

			Stream stream;
			if (apiUrl.ToString().Length > 2000)
			{
				stream = await PostAsync(apiUrl);
			}
			else
			{
				stream = await GetAsync(apiUrl);
			}

			XmlSerializer xml = new XmlSerializer(typeof(EveCentralApiQuickLookResult));
			var results = (EveCentralApiQuickLookResult)xml.Deserialize(stream);
			return results.quicklook;
		}


		public QuickLookPathResult QuickLookPath(string start, string end, int type, int setminQ = 10001, int sethours = 360)
		{
			Uri apiUrl = BuildQuickLookPathUrl(start, end, type, setminQ, sethours);

			var stream = Get(apiUrl);
			XmlSerializer xml = new XmlSerializer(typeof(EveCentralApiQuickLookPathResult));
			var results = (EveCentralApiQuickLookPathResult)xml.Deserialize(stream);
			return results.quicklook;
		}

		private Uri BuildQuickLookPathUrl(string start, string end, int type, int setminQ, int sethours)
		{
			string apiRelativeUrl = String.Format("quicklook/onpath/from/{0}/to/{1}/fortype/{2}", start, end, type);

			var args = HttpUtility.ParseQueryString(string.Empty);

			if (setminQ != 10001)
				args.Add("setminQ", setminQ.ToString());

			if (sethours != 360)
				args.Add("sethours", sethours.ToString());

			Uri apiUrl = new Uri(String.Format(ApiFormat, apiRelativeUrl, args.ToString()));
			return apiUrl;
		}

		public async Task<QuickLookPathResult> QuickLookPathAsync(string start, string end, int type, int setminQ = 10001, int sethours = 360)
		{
			Uri apiUrl = BuildQuickLookPathUrl(start, end, type, setminQ, sethours);

			var stream = await GetAsync(apiUrl);
			XmlSerializer xml = new XmlSerializer(typeof(EveCentralApiQuickLookPathResult));
			var results = (EveCentralApiQuickLookPathResult)xml.Deserialize(stream);
			return results.quicklook;
		}



		public List<TypeHistory> History(int type, LocaleType locale, string idOrName, OrderType bid)
		{
			Uri apiUrl = BuildHistoryUrl(type, locale, idOrName, bid);

			var stream = Get(apiUrl);
			StreamReader reader = new StreamReader(stream);
			string json = reader.ReadToEnd();
			return ParseHistoryJson(json);
		}

		private List<TypeHistory> ParseHistoryJson(string json)
		{
			List<TypeHistory> output = new List<TypeHistory>();
			dynamic parsed = JObject.Parse(json);
			dynamic values = parsed.values;
			foreach (var item in values)
			{
				TypeHistory derp = new TypeHistory();
				derp.Median = (float)item.median;
				derp.Maximum = (float)item.max;
				derp.Average = (float)item.avg;
				derp.StandardDeviation = (float)item.stdDev;
				derp.Minimum = (float)item.min;
				derp.Volume = (long)item.volume;
				derp.FivePercent = (float)item.fivePercent;
				derp.At = (DateTime)item.at;
				output.Add(derp);
			}
			return output;
		}

		private Uri BuildHistoryUrl(int type, LocaleType locale, string idOrName, OrderType bid)
		{
			string apiRelativeUrl = String.Format("history/for/type/{0}/{1}/{2}/bid/{3}", type, locale.ToString().ToLower(), idOrName, (int)bid);

			Uri apiUrl = new Uri(String.Format(ApiFormat, apiRelativeUrl, String.Empty));
			return apiUrl;
		}

		public async Task<List<TypeHistory>> HistoryAsync(int type, LocaleType locale, string idOrName, OrderType bid)
		{
			Uri apiUrl = BuildHistoryUrl(type, locale, idOrName, bid);

			var stream = await GetAsync(apiUrl);
			StreamReader reader = new StreamReader(stream);
			string json = await reader.ReadToEndAsync();
			return ParseHistoryJson(json);
		}

		public EveMonResult EveMon()
		{
			Uri apiUrl = BuildEveMonUrl();

			var stream = Get(apiUrl);
			XmlSerializer xml = new XmlSerializer(typeof(EveMonResult));
			var results = (EveMonResult)xml.Deserialize(stream);
			return results;
		}

		private Uri BuildEveMonUrl()
		{
			string apiRelativeUrl = "evemon";
			Uri apiUrl = new Uri(String.Format(ApiFormat, apiRelativeUrl, String.Empty));
			return apiUrl;
		}

		public async Task<EveMonResult> EveMonAsync()
		{
			Uri apiUrl = BuildEveMonUrl();

			var stream = await GetAsync(apiUrl);
			XmlSerializer xml = new XmlSerializer(typeof(EveMonResult));
			var results = (EveMonResult)xml.Deserialize(stream);
			return results;
		}

		public List<RouteJump> Route(string start, string end)
		{
			Uri apiUrl = BuildRouteUrl(start, end);

			var stream = Get(apiUrl);
			StreamReader reader = new StreamReader(stream);
			string json = reader.ReadToEnd();
			return ParseRouteJson(json);
		}

		private List<RouteJump> ParseRouteJson(string json)
		{
			List<RouteJump> output = new List<RouteJump>();
			dynamic parsed = JArray.Parse(json);
			foreach (var item in parsed)
			{
				var derp = JsonConvert.DeserializeObject<RouteJump>(item.ToString());
				output.Add(derp);
			}
			return output;
		}

		private Uri BuildRouteUrl(string start, string end)
		{
			string apiRelativeUrl = String.Format("route/from/{0}/to/{1}", start, end);

			Uri apiUrl = new Uri(String.Format(ApiFormat, apiRelativeUrl, String.Empty));
			return apiUrl;
		}

		public async Task<List<RouteJump>> RouteAsync(string start, string end)
		{
			Uri apiUrl = BuildRouteUrl(start, end);

			var stream = await GetAsync(apiUrl);
			StreamReader reader = new StreamReader(stream);
			string json = await reader.ReadToEndAsync();
			return ParseRouteJson(json);
		}

		private Stream Get(Uri apiUrl)
		{
			HttpWebRequest req = WebRequest.CreateHttp(apiUrl);
			req.UserAgent = UserAgent;
			req.Method = "GET";

			using (var response = req.GetResponse())
			{
				var stream = CopyAndClose(response.GetResponseStream());
				return stream;
			}

		}

		private async Task<Stream> GetAsync(Uri apiUrl)
		{
			HttpWebRequest req = WebRequest.CreateHttp(apiUrl);
			req.UserAgent = UserAgent;
			req.Method = "GET";

			using (var response = await req.GetResponseAsync())
			{
				var stream = CopyAndClose(response.GetResponseStream());
				return stream;
			}

		}

		/// <summary>
		/// This parses the query string on the apiUrl and turns it into POST data
		/// </summary>
		/// <param name="apiUrl">The URL for the request, which MUST include a query string with relavant POST data</param>
		/// <returns>The response stream.</returns>
		private Stream Post(Uri apiUrl)
		{
			var data = Encoding.UTF8.GetBytes(apiUrl.Query.TrimStart('?'));

			Uri requestUri = new Uri(apiUrl.GetLeftPart(UriPartial.Path));

			HttpWebRequest req = WebRequest.CreateHttp(requestUri);
			req.UserAgent = UserAgent;
			req.Method = "POST";
			req.ContentLength = data.Length;
			req.ContentType = "application/x-www-form-urlencoded";

			using (var reqStream = req.GetRequestStream())
			{
				reqStream.Write(data, 0, data.Length);
			}

			MemoryStream output = new MemoryStream();
			using (var response = req.GetResponse())
			{
				var stream = CopyAndClose(response.GetResponseStream());
				return stream;
			}
		}
		private async Task<Stream> PostAsync(Uri apiUrl)
		{
			var data = Encoding.UTF8.GetBytes(apiUrl.Query.TrimStart('?'));

			Uri requestUri = new Uri(apiUrl.GetLeftPart(UriPartial.Path));

			HttpWebRequest req = WebRequest.CreateHttp(requestUri);
			req.UserAgent = UserAgent;
			req.Method = "POST";
			req.ContentLength = data.Length;
			req.ContentType = "application/x-www-form-urlencoded";

			using (var reqStream = await req.GetRequestStreamAsync())
			{
				await reqStream.WriteAsync(data, 0, data.Length);
			}

			using (var response = await req.GetResponseAsync())
			{
				var stream = CopyAndClose(response.GetResponseStream());
				return stream;
			}
		}

		// http://stackoverflow.com/questions/147941/how-can-i-read-an-http-response-stream-twice-in-c
		// I have to copy the HttpResponse stream. Otherwise, the stream is closed when the response is closed by the using block.
		private static Stream CopyAndClose(Stream inputStream)
		{
			const int readSize = 256;
			byte[] buffer = new byte[readSize];
			MemoryStream ms = new MemoryStream();

			int count = inputStream.Read(buffer, 0, readSize);
			while (count > 0)
			{
				ms.Write(buffer, 0, count);
				count = inputStream.Read(buffer, 0, readSize);
			}
			ms.Position = 0;
			inputStream.Close();
			return ms;
		}

	}
}
