using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C38 RID: 3128
	public class RoadInfo : MapComponent
	{
		// Token: 0x04002F1A RID: 12058
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();

		// Token: 0x060044FE RID: 17662 RVA: 0x00244D9C File Offset: 0x0024319C
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x00244DB1 File Offset: 0x002431B1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, new object[0]);
		}
	}
}
