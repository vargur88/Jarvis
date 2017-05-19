using PriceMonitor.UI.UiViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace PriceMonitor.UI.UiViews
{
	/// <summary>
	/// Interaction logic for ShopListView.xaml
	/// </summary>
	public partial class ShopListView : UserControl
	{
		public ShopListView()
		{
			InitializeComponent();
		}

		private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var viewModel = this.DataContext as ShopListViewModel;
				viewModel?.FindItemByNameAsync(viewModel.SearchingItemName.Trim());
			}
		}
	}
}
