namespace PriceMonitor.UI.UiViewModels
{
	public class BasicReportViewModel : BaseViewModel
	{
		private readonly string _loadMessage = "Loading...";

		public BasicReportViewModel()
		{
			Report = new BasicReportData()
			{
				ItemName = _loadMessage,
				SellStation = _loadMessage,
				BuyStation = _loadMessage
			};
			/*
			task.ContinueWith(t =>
			{
				if (t.IsCompleted)
				{
					Report = t.Result;
				}
			});*/
		}

		private BasicReportData _report;
		public BasicReportData Report
		{
			get => _report;
			set
			{
				_report = value;
				NotifyPropertyChanged();
			}
		}

		public void AssignReport(BasicReportData report)
		{
			Report = report;
		}
	}
}
