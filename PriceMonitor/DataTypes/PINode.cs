using System.Collections.Generic;

namespace PriceMonitor.DataTypes
{
	public class PINode
	{
		public List<int> From = null;
		public List<int> To = null;

		public string Name { get; set; }
		public int ID { get; set; }

		public static List<PINode> CreatePINodes()
		{
			return new List<PINode>
			{
				new PINode()
				{
					Name = nameof(PIConst.BroadcastNode),
					ID = PIConst.BroadcastNodeID,
					From = new List<int>() {PIConst.BiotechResearchReportsID, PIConst.CameraDronesID}
				}
			};
		}
	}

	public static class PIConst
	{
		//tier4
		public static string BroadcastNode { get; }
		public static int BroadcastNodeID { get; set; } = 42;

		//tier3
		public static string BiotechResearchReports { get; }
		public static int BiotechResearchReportsID { get; set; } = 41;

		public static string CameraDrones { get; }
		public static int CameraDronesID { get; set; } = 40;
	}
}
