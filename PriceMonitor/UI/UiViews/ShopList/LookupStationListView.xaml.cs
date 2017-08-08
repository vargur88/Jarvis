using System.Windows;
using PriceMonitor.UI.UiViewModels;
using System.Windows.Controls;

namespace PriceMonitor.UI.UiViews
{
	/// <summary>
	/// Interaction logic for LookupStationListView.xaml
	/// </summary>
	public partial class LookupStationListView// : UserControl
	{
		public LookupStationListView()
		{
			InitializeComponent();
		}

		private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
		{
			(this.DataContext as LookupStationListViewModel).CancelAction();

			var parentWindow = Window.GetWindow((DependencyObject)sender);
			parentWindow?.Close();
		}

		private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
		{
			var parentWindow = Window.GetWindow((DependencyObject)sender);
			parentWindow?.Close();
		}
	}
}
