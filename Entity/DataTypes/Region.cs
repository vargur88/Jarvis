namespace Entity.DataTypes
{
	public class Region
	{
		public string Name { get; set; }
		public int RegionId { get; set; }
	}

	public class Station
	{
		public string Name { get; set; }
		public long StationId { get; set; } 
		public int RegionId { get; set; }
		public int SystemId { get; set; }

		public static Station GetJita()
		{
			return new Station()
			{
				Name = "Jita",
				SystemId = 10000002,
				RegionId = 10000002
			};
		}
	}

	public class SolarSystem
	{
		public string Name { get; set; }
		public int RegionId { get; set; }
		public int SystemId { get; set; }
	}
}
