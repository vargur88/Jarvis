using System.Windows;
using Entity.DataTypes;

namespace PriceMonitor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			this.DataContext =  new MainWindowViewModel();
			InitializeComponent();
		}

		private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var viewModel = this.DataContext as MainWindowViewModel;

			if (e.NewValue != null)
			{
				viewModel.SelectedNode = (ObjectsNode)e.NewValue;
			}
		}
	}
}
