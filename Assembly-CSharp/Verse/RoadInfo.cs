using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C39 RID: 3129
	public class RoadInfo : MapComponent
	{
		// Token: 0x04002F21 RID: 12065
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();

		// Token: 0x060044FE RID: 17662 RVA: 0x0024507C File Offset: 0x0024347C
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x00245091 File Offset: 0x00243491
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, new object[0]);
		}
	}
}
