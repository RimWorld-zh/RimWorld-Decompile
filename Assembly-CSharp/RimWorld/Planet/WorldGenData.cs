using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BC RID: 1468
	public class WorldGenData : WorldComponent
	{
		// Token: 0x06001C2A RID: 7210 RVA: 0x000F24C3 File Offset: 0x000F08C3
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000F24E3 File Offset: 0x000F08E3
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
