using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

			viewModel?.ShowHistory(true);
		}

		private bool _selected = false;
		private void ExpanderPI_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			_selected = !_selected;

			var viewModel = this.DataContext as ItemTinyTradeHistoryViewModel;
			viewModel?.UpdatePiChain(_selected);
		}

		private void ExpanderPI_OnMouseEnter(object sender, MouseEventArgs e)
		{
			var viewModel = this.DataContext as ItemTinyTradeHistoryViewModel;
			viewModel?.UpdateFocus(true);
		}

		private void ExpanderPI_OnMouseLeave(object sender, MouseEventArgs e)
		{
			var viewModel = this.DataContext as ItemTinyTradeHistoryViewModel;
			viewModel?.UpdateFocus(false);
		}
	}
}
