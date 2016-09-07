using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveCentralProvider.Types
{
	public class TypeHistory
	{
		public float Median { get; set; }

		public float Maximum { get; set; }

		public float Average { get; set; }

		public float StandardDeviation { get; set; }

		public float Minimum { get; set; }

		public long Volume { get; set; }

		public float FivePercent { get; set; }

		public DateTime At { get; set; }
	}
}
