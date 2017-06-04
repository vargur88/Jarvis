﻿using System.Collections.Generic;
using Entity.DataTypes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Entity;
using Helpers;
using System.Data;

namespace PriceMonitor.UI.UiViewModels
{
	public class ShopListViewModel : BaseViewModel
	{
		public ShopListViewModel()
		{
			OrdersInfoList = new List<OrderItemInfo>()
			{
				new OrderItemInfo()
				{
					StationName = "Jita",
					BuyList = new List<OrderInfo>()
					{
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						},
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						}
					},
					SellList = new List<OrderInfo>()
					{
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						},
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						}
					}
				},
				new OrderItemInfo()
				{
					StationName = "Hek",
					BuyList = new List<OrderInfo>()
					{
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						},
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						}
					},
					SellList = new List<OrderInfo>()
					{
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						},
						new OrderInfo()
						{
							ItemsCount = 10,
							ItemPrice = 123
						}
					}
				}
			};
		}

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
				AggregateList.Clear();


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

		private List<OrderItemInfo> _ordersInfoList = new List<OrderItemInfo>();
		public List<OrderItemInfo> OrdersInfoList
		{
			get => _ordersInfoList;
			set
			{
				_ordersInfoList = value;
				NotifyPropertyChanged();
			}
		}

		public struct OrderItemInfo
		{
			public GameObject Object { get; set; }
			public string StationName { get; set; }

			public List<OrderInfo> SellList { get; set; }
			public List<OrderInfo> BuyList { get; set; }
		}

		public struct OrderInfo
		{
			public int ItemsCount { get; set; }
			public float ItemPrice { get; set; }
		}

		private DataTable _aggregateList;
		public DataTable AggregateList
		{
			get => _aggregateList;
			set
			{
				_aggregateList = value;
				NotifyPropertyChanged();
			}
		}
	}
}
