using System;
using Entity.DataTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using EveCentralProvider;
using Helpers;
using PriceMonitor.DataTypes;
using PriceMonitor.UI.UiViewModels;
using System.Windows;

namespace PriceMonitor
{
	public class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			Task.Run(() =>
			{
				var regionList = EntityService.Instance.RequestRegionsAsync().Result;

				Application.Current.Dispatcher.Invoke(() =>
				{
					Charts.Add(new ChartViewModel(regionList));
					Charts.Add(new ChartViewModel(regionList));
				});

				//MenuItems = EntityService.Instance.RequestChainAsync().Result;
			});
		}

		private ObservableCollection<ChartViewModel> _charts = new ObservableCollection<ChartViewModel>();

		public ObservableCollection<ChartViewModel> Charts
		{
			get { return _charts; }
			set
			{
				_charts = value;
				NotifyPropertyChanged();
			}
		}

		private IList<ObjectsChain> _menuItems;
		public IList<ObjectsChain> MenuItems
		{
			get { return _menuItems; }
			set
			{
				_menuItems = value;
				NotifyPropertyChanged();
			}
		}

		private ObjectsChain _selectedChain;
		public ObjectsChain SelectedChain
		{
			get { return _selectedChain; }
			set
			{
				_selectedChain = value;
				NotifyPropertyChanged();
			}
		}

		private RelayCommand _checkPriceCmd;
		public RelayCommand CheckPriceCmd
		{
			get
			{
				return _checkPriceCmd ?? (_checkPriceCmd = new RelayCommand(/*p => SelectedChain != null,*/ p => CheckPrice(SelectedChain, null, null)));
			}
		}

		private void CheckPrice(ObjectsChain chainToCheck, Region first, Region second)
		{/*
			if (chainToCheck.Object.TypeId == 0)
			{
				return;
			}*/

			Task.Run( () =>
			{
				try
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						//Charts.Clear();
						//Charts.Add(new ChartViewModel(chart));
					});
				}
				catch (Exception e)
				{
				}
			});
		}
	}
}
