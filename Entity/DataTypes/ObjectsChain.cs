using System.Collections.Generic;

namespace Entity.DataTypes
{
	public class GameObject
	{
		public string Name { get; set; }
		public int MarketGroupId { get; set; }
		public int TypeId { get; set; }
	}

	public class ObjectsChain
	{
		public GameObject Object { get; set; }
		public IList<ObjectsChain> SubObjects { get; set; }
	}
}
