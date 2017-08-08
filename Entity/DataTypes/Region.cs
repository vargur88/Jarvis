namespace Entity.DataTypes
{
	public class CommonMapObject
	{
		public string Name { get; set; }
		public long Id { get; set; }
	}

	public class Region
	{
		public string Name { get; set; }
		public int RegionId { get; set; }
	}

	public class Station
	{
		public string StationName { get; set; }
		public long StationId { get; set; }

		public string RegionName { get; set; }
		public int RegionId { get; set; }

		public string SystemName { get; set; }
		public int SystemId { get; set; }

		public static Station GetJita()
		{
			return new Station()
			{
				StationName = "Jita",
				SystemId = 10000002,
				RegionId = 10000002
			};
		}

		public string ShortName()
		{
			return StationName.Substring(0, StationName.IndexOf(' '));
		}
	}

	public class SolarSystem
	{
		public string Name { get; set; }
		public int RegionId { get; set; }
		public int SystemId { get; set; }
	}
}
