using System.Collections.Generic;
using Entity.DataTypes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Entity;
using Helpers;

namespace PriceMonitor.UI.UiViewModels
{
	public class ShopListViewModel : BaseViewModel
	{
		public async Task FindItemByNameAsync(string itemName)
		{
			lock (ShopList)
			{
				SearchingItemName = "";

				if (ShopList.Any(t => t.Name != itemName))
				{
					return;
				}
			}

			await EntityService.Instance.RequestObjectAsync(itemName)
				.ContinueWith(t =>
				{
					if (t == null)
					{
						return;
					}

					Application.Current.Dispatcher.Invoke(() =>
					{
						lock (ShopList)
						{
							ShopList.Add(t.Result);
						}
					});
				});
		}

		private RelayCommand _clearCmd;
		public RelayCommand ClearCmd
		{
			get
			{
				return _clearCmd ?? (_clearCmd = new RelayCommand(t =>
				{
					lock (ShopList)
					{
						ShopList.Clear();
					}
				}));
			}
		}

		private RelayCommand _reviewCmd;
		public RelayCommand ReviewCmd
		{
			get
			{
				return _reviewCmd ?? (_reviewCmd = new RelayCommand(async t =>
				{
					var stationList = new ObservableCollection<CommonMapObject>();

					var win = new Window
					{
						Content = new LookupStationListViewModel(ref stationList),
						SizeToContent = SizeToContent.WidthAndHeight,
						WindowStartupLocation = WindowStartupLocation.CenterScreen,
						ResizeMode = ResizeMode.NoResize,
						WindowStyle = WindowStyle.None
					};
					win.ShowDialog();

					List<GameObject> shopList;
					lock (ShopList)
					{
						shopList = ShopList.ToList();
					}

					await GenerateReviewReportAsync(stationList.ToList(), shopList).ConfigureAwait(false);
				}));
			}
		}

		private async Task GenerateReviewReportAsync(List<CommonMapObject> stationList, List<GameObject> shopList)
		{
			if (stationList.Any() && shopList.Any())
			{

			}
		}

		private string _searchingItemName;
		public string SearchingItemName
		{
			get => _searchingItemName;
			set
			{
				if (_searchingItemName == value)
				{
					return;
				}
				_searchingItemName = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<GameObject> _shopList = new ObservableCollection<GameObject>();
		public ObservableCollection<GameObject> ShopList
		{
			get => _shopList;
			set
			{
				_shopList = value;
				NotifyPropertyChanged();
			}
		}
	}
}
