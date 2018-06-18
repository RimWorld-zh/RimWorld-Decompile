using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C39 RID: 3129
	public class RoadInfo : MapComponent
	{
		// Token: 0x060044F2 RID: 17650 RVA: 0x002438F0 File Offset: 0x00241CF0
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00243905 File Offset: 0x00241D05
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, new object[0]);
		}

		// Token: 0x04002F10 RID: 12048
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
