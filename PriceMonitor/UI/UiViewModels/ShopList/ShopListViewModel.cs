using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Entity.DataTypes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Entity;
using Helpers;
using System.Data;
using EveCentralProvider;
using EveCentralProvider.Types;

namespace PriceMonitor.UI.UiViewModels
{
	public class ShopListViewModel : BaseViewModel
	{
		public async Task FindItemByNameAsync(string itemName)
		{
			lock (ShopList)
			{
				SearchingItemName = "";

				if (ShopList.Any(t => t.Name == itemName))
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
					var win = new Window
					{
						Content = new LookupStationListViewModel(ref _stationList),
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

					await GenerateReviewReportAsync(_stationList.ToList(), shopList).ConfigureAwait(false);
				}));
			}
		}

		private async Task GenerateReviewReportAsync(List<Station> stationList, List<GameObject> itemList)
		{
			if (stationList.Any() && itemList.Any())
			{
				await Task.Run(() =>
				{
					DataTable table = new DataTable();
					table.Columns.Add("Item");

					var aggregates = new Dictionary<int, List<AggreateInfoStat>>();
					foreach (var item in itemList)
					{
						aggregates[item.TypeId] = new List<AggreateInfoStat>(stationList.Count);
					}

					var waitList = new List<Task>();
					foreach (var station in stationList)
					{
						var formatRegionName = station.Name.Substring(0, station.Name.IndexOf(' '));
						table.Columns.Add(formatRegionName);

						waitList.Add(Services.Instance.AggregateInfoAsync(itemList.Select(t => t.TypeId).ToList(), (int)station.StationId)
							.ContinueWith(t =>
							{
								if (t.IsFaulted)
								{
									return;
								}

								if (t.IsCompleted)
								{
									foreach (var item in t.Result.Items)
									{
										// not thread safe
										item.sell.RegionName = formatRegionName;

										lock (aggregates)
										{
											aggregates[Convert.ToInt32(item.id)].Add(item.sell);
										}
									}
								}
							}));
					}

					//improvement: do not wait all of the tasks, start to draw results ASAP
					Task.WaitAll(waitList.ToArray());

					foreach (var item in itemList)
					{
						var nextRow = table.NewRow();

						int index = 0;
						nextRow[index++] = item.Name;

						var list = aggregates[item.TypeId];
						for (int i = 0; i < stationList.Count; ++i)
						{
							var regionPrice = list.SingleOrDefault(t => t.RegionName == table.Columns[i + 1].ColumnName);
							if (regionPrice != null)
							{
								nextRow[index] = regionPrice.percentile;
							}

							index++;
						}

						table.Rows.Add(nextRow);
					}

					Application.Current.Dispatcher.Invoke(() =>
					{
						AggregateList = table;
					});
				})
				.ConfigureAwait(false);
			}
		}

		private DataRowView _selectedItem;
		public DataRowView SelectedItem
		{
			get => _selectedItem;
			set
			{
				if (Equals(_selectedItem, value))
				{
					return;
				}
				_selectedItem = value;
				NotifyPropertyChanged();

				RequestOrdersForItemAsync(SelectedItem[0] as string);
			}
		}

		private async void RequestOrdersForItemAsync(string itemName)
		{
			lock (_allOrdersInfoList)
			{
				var itemInfo = _allOrdersInfoList.Where(t => t.Object.Name == itemName).ToList();
				if (itemInfo.Any())
				{
					SelectedItemOrders = new ObservableCollection<OrderItemInfo>(itemInfo);
					return;
				}
			}

			SelectedItemOrders.Clear();

			await Task.Run(() =>
			{
				OrderInfo ItemConvert(Order order)
				{
					return new OrderInfo()
					{
						ItemPrice = (float)Math.Round(order.Price / 1000000, 4),
						ItemsCount = (int)order.VolumeRemaining
					};
				}

				foreach (var station in _stationList)
				{
					var item = ShopList.Single(t => t.Name == itemName);
					var result = Services.Instance.QuickLook(item.TypeId, new List<int>() {station.RegionId}, 1, station.SystemId);

					var ordersInfo = new OrderItemInfo() {Object = item, StationName = station.Name};

					if (result.SellOrders != null && result.SellOrders.Any())
					{
						ordersInfo.SellList = result.SellOrders.OrderBy(k => k.Price).Take(5).Select(ItemConvert).ToList();
					}
					if (result.BuyOrders != null && result.BuyOrders.Any())
					{
						ordersInfo.BuyList = result.BuyOrders.OrderByDescending(k => k.Price).Take(5).Select(ItemConvert).ToList();
					}

					lock (_allOrdersInfoList)
					{
						_allOrdersInfoList.Add(ordersInfo);

						Application.Current.Dispatcher.Invoke(() =>
						{
							SelectedItemOrders.Add(ordersInfo);
						});
					}
				}
			})
			.ConfigureAwait(false);
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

		private ObservableCollection<Station> _stationList = new ObservableCollection<Station>();

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

		private List<OrderItemInfo> _allOrdersInfoList = new List<OrderItemInfo>();

		private ObservableCollection<OrderItemInfo> _selectedItemOrders = new ObservableCollection<OrderItemInfo>();
		public ObservableCollection<OrderItemInfo> SelectedItemOrders
		{
			get => _selectedItemOrders;
			set
			{
				_selectedItemOrders = value;
				NotifyPropertyChanged();
			}
		}

		public class OrderItemInfo
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
