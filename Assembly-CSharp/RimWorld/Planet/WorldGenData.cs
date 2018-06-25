using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BA RID: 1466
	public class WorldGenData : WorldComponent
	{
		// Token: 0x040010E3 RID: 4323
		public List<int> roadNodes = new List<int>();

		// Token: 0x040010E4 RID: 4324
		public List<int> ancientSites = new List<int>();

		// Token: 0x06001C26 RID: 7206 RVA: 0x000F2947 File Offset: 0x000F0D47
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000F2967 File Offset: 0x000F0D67
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, new object[0]);
		}
	}
}
