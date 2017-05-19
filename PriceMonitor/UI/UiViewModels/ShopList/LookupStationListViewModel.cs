using System;
using Entity.DataTypes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Entity;
using Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class LookupStationListViewModel : BaseViewModel
	{
		public Action CloseAction { get; set; }

		private RelayCommand _closeCmd;
		public RelayCommand CloseCmd
		{
			get
			{
				return _closeCmd ?? (_closeCmd = new RelayCommand(t =>
				{
				}));
			}
		}
	}
}
