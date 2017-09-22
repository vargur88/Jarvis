using System;
using System.Collections.Generic;
using Entity.DataTypes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Entity;
using Helpers;
using System.Data;
using System.IO;
using System.Windows.Data;
using EveCentralProvider;
using EveCentralProvider.Types;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace PriceMonitor.UI.UiViewModels
{
	public class ShopListViewModel : BaseViewModel
	{
		public ShopListViewModel()
		{
			_jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.Objects,
				TypeNameHandling = TypeNameHandling.Auto,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize
			};

			foreach (var nextFile in Directory.EnumerateFiles(Path.Combine(Resource1.UserData, Resource1.ShopList)))
			{
				try
				{
					SavedLists.Add(Deserialize<SavableShopList>(File.ReadAllText(nextFile)));
				}
				catch (Exception e)
				{}
			}

			CreateModel();
		}

		private void CreateModel()
		{
			Model = new PlotModel
			{
				LegendBorder = OxyColors.Aqua,
				LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
				LegendPosition = LegendPosition.LeftBottom,
				LegendPlacement = LegendPlacement.Inside,
				Axes =
				{
					new LinearAxis()
					{
						Position = AxisPosition.Right,
						MajorGridlineStyle = LineStyle.Solid,
						MinorGridlineStyle = LineStyle.Solid,
						MajorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						MinorTicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						ExtraGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						AxislineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						TicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						MinorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						TextColor = OxyColors.Aqua
					},
					new DateTimeAxis()
					{
						Position = AxisPosition.Bottom,
						MajorGridlineStyle = LineStyle.Solid,
						MinorGridlineStyle = LineStyle.Solid,
						MajorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						MinorTicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						ExtraGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						AxislineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						TicklineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						MinorGridlineColor = OxyColor.FromRgb(0x3a,0x3a,0x3a),
						TextColor = OxyColors.Aqua
					}
				}
			};
		}

		public CollectionView TimeFilters { get; } = new CollectionView(new List<TimeFilter>()
		{
			new TimeFilter(TimeFilter.TimeFilterEnum.Day),
			new TimeFilter(TimeFilter.TimeFilterEnum.Week),
			new TimeFilter(TimeFilter.TimeFilterEnum.Month),
			new TimeFilter(TimeFilter.TimeFilterEnum.Quarter),
			new TimeFilter(TimeFilter.TimeFilterEnum.Year),
			new TimeFilter(TimeFilter.TimeFilterEnum.AllTime),
		});

		private void RequestHistory(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}

			// chache for history here

			Model.Series.Clear();
			var item = ShopList.Single(t => t.Name == name);

			Task.Run(async () =>
			{
				foreach (var station in _stationList)
				{
					var historyResponse = await Services.Instance.HistoryAsync(item.TypeId, station.RegionId);

					var dataPoints =
						historyResponse.Items.Select(t => new DataPoint(DateTimeAxis.ToDouble(t.Date), t.AvgPrice)).ToList();

					var hubChart = new LineSeries
					{
						Title = station.ShortName(),
						Color = OxyColors.Automatic
					};
					hubChart.Points.AddRange(dataPoints);

					Application.Current.Dispatcher.Invoke(() =>
					{
						Model.Series.Add(hubChart);
						UpdateTimeAxis((int) SelectedTimeFilter.Value);
					});
				}
			})
			.ConfigureAwait(false);
		}

		private TimeFilter _selectedTimeFilter = new TimeFilter(TimeFilter.TimeFilterEnum.Month);
		public TimeFilter SelectedTimeFilter
		{
			get => _selectedTimeFilter;
			set
			{
				if (_selectedTimeFilter == value)
				{
					return;
				}

				_selectedTimeFilter = value;
				NotifyPropertyChanged();

				UpdateTimeAxis((int)SelectedTimeFilter.Value);
			}
		}

		private void UpdateTimeAxis(int days)
		{
			if (Model != null)
			{
				Model.Axes[1].FilterMinValue = days == 0 ? 0 : DateTimeAxis.ToDouble(DateTime.Now - TimeSpan.FromDays(days));

				Model.ResetAllAxes();
				Model.InvalidatePlot(true);
			}
		}

		private PlotModel model;
		public PlotModel Model
		{
			get { return model; }
			set
			{
				if (model != value)
				{
					model = value;
					NotifyPropertyChanged();
				}
			}
		}

		public override void Dispose()
		{
			foreach (var list in SavedLists)
			{
				TextWriter writer = null;
				try
				{
					var json = JsonConvert.SerializeObject(list);
					writer = new StreamWriter(Path.Combine(Resource1.UserData, Resource1.ShopList, list.Name));
					writer.Write(json);
				}
				finally
				{
					writer?.Close();
				}
			}
		}

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
				return _clearCmd ?? (_clearCmd = new RelayCommand(t => ShopList.Any(), t =>
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
				return _reviewCmd ?? (_reviewCmd = new RelayCommand(t => ShopList.Any(), async t =>
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

		private RelayCommand _saveToListCmd;
		public RelayCommand SaveToListCmd
		{
			get
			{
				return _saveToListCmd ?? (_saveToListCmd = new RelayCommand(t => ShopList.Any(), t =>
				{
					SavedLists.Add(new SavableShopList()
					{
						Name = "New List" + SavedLists.Count,
						Objects = ShopList.ToList()
					});
				}));
			}
		}

		private RelayCommand _deleteListCmd;
		public RelayCommand DeleteListCmd
		{
			get
			{
				return _deleteListCmd ?? (_deleteListCmd = new RelayCommand(t => SelectedShopList != null, t =>
				{
					var name = SelectedShopList.Name;
					SavedLists.Remove(SavedLists.Single(p => p.Name == name));

					try
					{
						File.Delete(Path.Combine(Resource1.UserData, Resource1.ShopList, name));
					}
					catch(Exception e)
					{}
				}));
			}
		}

		private RelayCommand _loadListCmd;
		public RelayCommand LoadListCmd
		{
			get
			{
				return _loadListCmd ?? (_loadListCmd = new RelayCommand(t => SelectedShopList != null, t =>
				{
					ShopList = new ObservableCollection<GameObject>(SelectedShopList.Objects);
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
						table.Columns.Add(station.RegionName);

						waitList.Add(Services.Instance.AggregateInfoAsync(itemList.Select(t => t.TypeId).ToList(), station.RegionId)
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
										item.sell.RegionName = station.RegionName;

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
								nextRow[index] = Math.Round(regionPrice.percentile, 2);
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
				RequestHistory(SelectedItem[0] as string);
			}
		}

		private async void RequestOrdersForItemAsync(string itemName)
		{
			lock (_cacheOrdersInfoList)
			{
				var itemInfo = _cacheOrdersInfoList.Where(t => t.Object.Name == itemName).ToList();
				if (itemInfo.Any())
				{
					SelectedItemOrders = new ObservableCollection<OrderItemInfo>(itemInfo);
					return;
				}
			}

			SelectedItemOrders.Clear();

			await Task.Run(async () =>
			{
				OrderInfo ItemConvert(OrderCrest order)
				{
					return new OrderInfo()
					{
						ItemPrice = (float)Math.Round(order.price / 1000000, 4),
						ItemsCount = (int)order.volume
					};
				}

				var jita = Station.GetJita();
				var item = ShopList.Single(t => t.Name == itemName);
				var resultSell = await Services.Instance.ViewOrdersAsync(item.TypeId, jita.RegionId, sell : true);
				var resultBuy = await Services.Instance.ViewOrdersAsync(item.TypeId, jita.RegionId, sell : false);

				var ordersInfo = new OrderItemInfo() { Object = item, StationName = jita.SystemName };

				if (resultSell?.items != null && resultSell.items.Count > 0)
				{
					ordersInfo.SellList = resultSell.items.OrderBy(k => k.price).Select(ItemConvert).ToList();
				}
				if (resultBuy?.items != null && resultBuy.items.Count > 0)
				{
					ordersInfo.BuyList = resultBuy.items.OrderByDescending(k => k.price).Select(ItemConvert).ToList();
				}

				lock (_cacheOrdersInfoList)
				{
					_cacheOrdersInfoList.Add(ordersInfo);

					Application.Current.Dispatcher.Invoke(() =>
					{
						SelectedItemOrders.Add(ordersInfo);
					});
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

		private ObservableCollection<SavableShopList> _savedList = new ObservableCollection<SavableShopList>();
		public ObservableCollection<SavableShopList> SavedLists
		{
			get => _savedList;
			set
			{
				_savedList = value;
				NotifyPropertyChanged();
			}
		}

		private SavableShopList _selectedShopList;
		public SavableShopList SelectedShopList
		{
			get => _selectedShopList;
			set
			{
				_selectedShopList = value;
				NotifyPropertyChanged();
			}
		}

		private readonly List<OrderItemInfo> _cacheOrdersInfoList = new List<OrderItemInfo>();

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

		private static JsonSerializerSettings _jsonSerializerSettings;

		private static T Deserialize<T>(string content)
		{
			return JsonConvert.DeserializeObject<T>(content, _jsonSerializerSettings);
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

		public class SavableShopList
		{
			public string Name { get; set; }
			public List<GameObject> Objects { get; set; }
		}

		public class TimeFilter
		{
			public TimeFilter(TimeFilterEnum value)
			{
				Value = value;
				Name = Value.ToString();
			}

			public string Name { get; set; }
			public TimeFilterEnum Value { get; set; }

			public enum TimeFilterEnum
			{
				Day = 2,
				Week = 7,
				Month = 30,
				Quarter = 120,
				Year = 365,
				AllTime = 0
			}
		}
	}
}
