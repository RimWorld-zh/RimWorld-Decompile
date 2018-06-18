using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BC RID: 1468
	public class WorldGenData : WorldComponent
	{
		// Token: 0x06001C2C RID: 7212 RVA: 0x000F253B File Offset: 0x000F093B
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000F255B File Offset: 0x000F095B
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, new object[0]);
		}

		// Token: 0x040010E2 RID: 4322
		public List<int> roadNodes = new List<int>();

		// Token: 0x040010E3 RID: 4323
		public List<int> ancientSites = new List<int>();
	}
}
