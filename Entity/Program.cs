using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var ctx = new EntitiesConnection())
			{
				var rootList = ctx.eve_inv_marketgroups.Where(t => t.parentgroup_id == 0).ToList();
				foreach (var item in rootList)
				{
					Console.WriteLine(item.marketgroup_name);
				}

				/*
				var autocannonID = ctx.eve_inv_marketgroups.SingleOrDefault(t => t.marketgroup_id == 559);
				var mediumCannon = ctx.eve_inv_marketgroups.SingleOrDefault(t => t.parent_id_1 == autocannonID.marketgroup_id && t.marketgroup_name == "Medium");
				var mediumT2AutoCannon = ctx.eve_inv_types.Where(t => t.marketgroup_id == mediumCannon.marketgroup_id && t.meta_level == 5).ToList();

				foreach (var gun in mediumT2AutoCannon)
				{
					Console.WriteLine(gun.name);
				}*/

				/*var r = ctx.eve_inv_types.SingleOrDefault(t => t.type_id == 2073);
				if (r != null)
				{
					Console.WriteLine(r.description);
					var m = ctx.eve_inv_types.Where(t => t.marketgroup_id == r.marketgroup_id).ToList();
					foreach (var eveInvTypese in m)
					{
						Console.WriteLine(eveInvTypese.description);
					}
				}*/

			}

			Console.ReadLine();
		}
	}
}
