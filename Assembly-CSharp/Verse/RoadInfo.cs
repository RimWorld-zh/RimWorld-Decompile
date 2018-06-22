using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C36 RID: 3126
	public class RoadInfo : MapComponent
	{
		// Token: 0x060044FB RID: 17659 RVA: 0x00244CC0 File Offset: 0x002430C0
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x00244CD5 File Offset: 0x002430D5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, new object[0]);
		}

		// Token: 0x04002F1A RID: 12058
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
