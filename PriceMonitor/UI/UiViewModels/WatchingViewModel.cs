using System.Collections.Generic;
using System.Threading.Tasks;
using EveCentralProvider;

namespace PriceMonitor.UI.UiViewModels
{
	public class WatchingViewModel : BaseViewModel
	{
		public WatchingViewModel()
		{
			Task.Run(async () =>
			{
				var k = await Services.Instance.HistoryAsync(2865, 10000002);
			}).Wait();
		}
	}
}
