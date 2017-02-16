using System.Windows;
using System.Windows.Controls;
using Entity.DataTypes;
using PriceMonitor.UI.UiViewModels;

namespace PriceMonitor.UI.UiViews
{
	/// <summary>
	/// Interaction logic for ItemTinyTradeHistoryView.xaml
	/// </summary>
	public partial class ItemTinyTradeHistoryView : UserControl
	{
		public ItemTinyTradeHistoryView()
		{
			InitializeComponent();
		}

		private void Expander_OnExpanded(object sender, RoutedEventArgs e)
		{
			var viewModel = this.DataContext as ItemTinyTradeHistoryViewModel;

			viewModel?.ChangeVisibility(true);
		}
	}
}
