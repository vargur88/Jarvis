using System;

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

		private void MainWindow_OnClosed(object sender, EventArgs e)
		{
			var viewModel = this.DataContext as MainWindowViewModel;
			viewModel?.Close();
		}
	}
}
