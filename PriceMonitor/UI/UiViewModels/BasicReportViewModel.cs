using System.Threading.Tasks;

namespace PriceMonitor.UI.UiViewModels
{
	public class BasicReportViewModel : BaseViewModel
	{
		private readonly string _loadMessage = "Loading...";

		public BasicReportViewModel(Task<BasicReportData> task)
		{
			Report = new BasicReportData()
			{
				ItemName = _loadMessage, SellStation = _loadMessage, BuyStation = _loadMessage
			};

			task.ContinueWith(t =>
			{
				if (t.IsCompleted)
				{
					Report = t.Result;
				}
			});
		}

		private BasicReportData _report;
		public BasicReportData Report
		{
			get { return _report; }
			set
			{
				_report = value;
				NotifyPropertyChanged();
			}
		}
	}
}
