using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C3A RID: 3130
	public class RoadInfo : MapComponent
	{
		// Token: 0x060044F4 RID: 17652 RVA: 0x00243918 File Offset: 0x00241D18
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0024392D File Offset: 0x00241D2D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, new object[0]);
		}

		// Token: 0x04002F12 RID: 12050
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
