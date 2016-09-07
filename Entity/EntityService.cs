using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.DataTypes;
using System.Data.Entity;
using System.Linq;

namespace Entity
{
	public sealed class EntityService
	{
		public static EntityService Instance { get; } = new EntityService();
		private EntityService(){}

		public async Task<List<ObjectsChain>> RequestChainAsync(int roolLevel = 0)
		{
			var rootList = await DoRequestChain(roolLevel);

			foreach (var item in rootList)
			{
				item.SubObjects = await DoRequestChain(item.Object.MarketGroupId);
			}

			return rootList;
		}

		private async Task<List<ObjectsChain>> DoRequestChain(int level)
		{
			using (var ctx = new EntitiesConnection())
			{
				var list = await ctx.eve_inv_marketgroups.Where(t => t.parentgroup_id == level)
					.Select(t => new ObjectsChain()
					{
						Object = new GameObject()
						{
							Name = t.marketgroup_name,
							MarketGroupId = t.marketgroup_id
						}
					})
					.ToListAsync();

				foreach (var item in list)
				{
					item.SubObjects = await DoRequestChain(item.Object.MarketGroupId);
				}

				if (list.Count != 0)
				{
					return list;
				}

				var objects = await RequestObjectsAsync(level);
				list = objects.Select(gameObject => new ObjectsChain() {Object = gameObject}).ToList();

				return list;
			}
		}

		public async Task<List<GameObject>> RequestObjectsAsync(int marketGroudpId)
		{
			using (var ctx = new EntitiesConnection())
			{
				var list = await ctx.eve_inv_types.Where(t => t.marketgroup_id == marketGroudpId)
					.Select(t => new GameObject()
					{
						Name = t.name,
						MarketGroupId = (int) t.marketgroup_id,
						TypeId = t.type_id
					})
					.ToListAsync();

				return list;
			}
		}
	}
}
