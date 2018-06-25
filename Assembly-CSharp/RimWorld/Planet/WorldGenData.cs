using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BA RID: 1466
	public class WorldGenData : WorldComponent
	{
		// Token: 0x040010DF RID: 4319
		public List<int> roadNodes = new List<int>();

		// Token: 0x040010E0 RID: 4320
		public List<int> ancientSites = new List<int>();

		// Token: 0x06001C27 RID: 7207 RVA: 0x000F26DF File Offset: 0x000F0ADF
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000F26FF File Offset: 0x000F0AFF
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, new object[0]);
		}
	}
}
