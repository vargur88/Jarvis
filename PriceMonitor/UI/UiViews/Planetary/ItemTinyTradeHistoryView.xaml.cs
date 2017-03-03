using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
		private Brush _defaultBrush;
		private readonly Brush _currentBrush = PickBrush();

		private void ExpanderPI_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_defaultBrush == null)
			{
				_defaultBrush = ExpanderPI.Background;
			}

			ExpanderPI.Background = !_selected ? _currentBrush : _defaultBrush;
			_selected = !_selected;

			var viewModel = this.DataContext as ItemTinyTradeHistoryViewModel;
			viewModel?.UpdatePiChain(_currentBrush, _selected);
		}

		private static readonly PropertyInfo[] Properties = typeof(Brushes).GetProperties();
		private static Brush PickBrush()
		{
			Random rnd = new Random();

			int random = rnd.Next(Properties.Length);

			return (Brush)Properties[random].GetValue(null, null);
		}
	}
}
