using System.Windows;
using System.Windows.Controls;
using Entity.DataTypes;
using PriceMonitor.UI.UiViewModels;

namespace PriceMonitor.UI.UiViews
{
	/// <summary>
	/// Interaction logic for ReportsView.xaml
	/// </summary>
	public partial class ReportsView : UserControl
	{
		public ReportsView()
		{
			InitializeComponent();
		}

		private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var viewModel = this.DataContext as ReportsViewModel;
			if (e.NewValue != null && viewModel != null)
			{
				viewModel.SelectedNode = (ObjectsNode)e.NewValue;
			}
		}
	}
}
