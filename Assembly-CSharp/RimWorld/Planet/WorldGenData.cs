using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B8 RID: 1464
	public class WorldGenData : WorldComponent
	{
		// Token: 0x040010DF RID: 4319
		public List<int> roadNodes = new List<int>();

		// Token: 0x040010E0 RID: 4320
		public List<int> ancientSites = new List<int>();

		// Token: 0x06001C23 RID: 7203 RVA: 0x000F258F File Offset: 0x000F098F
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x000F25AF File Offset: 0x000F09AF
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, new object[0]);
		}
	}
}
