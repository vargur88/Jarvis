using System.Windows;
using PriceMonitor.UI.UiViewModels;
using System.Windows.Controls;

namespace PriceMonitor.UI.UiViews
{
	/// <summary>
	/// Interaction logic for LookupStationListView.xaml
	/// </summary>
	public partial class LookupStationListView : UserControl
	{
		public LookupStationListView()
		{
			InitializeComponent();

			var vm = new LookupStationListViewModel();
			this.DataContext = vm;
		}
	}
}
