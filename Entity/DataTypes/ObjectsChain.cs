using System.Collections.Generic;

namespace Entity.DataTypes
{
	public class GameObject
	{
		public string Name { get; set; }
		public int MarketGroupId { get; set; }
		public int TypeId { get; set; }

		public static GameObject GetTritanium()
		{
			return new GameObject()
			{
				Name = "Tritanium",
				TypeId = 34,
				MarketGroupId = 1857
			};
		}
	}

	public class ObjectsChain
	{
		public GameObject Object { get; set; }
		public IList<ObjectsChain> SubObjects { get; set; }
	}
}
