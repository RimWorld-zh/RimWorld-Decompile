using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000736 RID: 1846
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x0400164C RID: 5708
		public ThingDef thingToSpawn;

		// Token: 0x0400164D RID: 5709
		public int spawnCount = 1;

		// Token: 0x0400164E RID: 5710
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x0400164F RID: 5711
		public int spawnMaxAdjacent = -1;

		// Token: 0x04001650 RID: 5712
		public bool spawnForbidden;

		// Token: 0x04001651 RID: 5713
		public bool requiresPower;

		// Token: 0x04001652 RID: 5714
		public bool writeTimeLeftToSpawn;

		// Token: 0x04001653 RID: 5715
		public bool showMessageIfOwned;

		// Token: 0x04001654 RID: 5716
		public string saveKeysPrefix;

		// Token: 0x060028BD RID: 10429 RVA: 0x0015BCDC File Offset: 0x0015A0DC
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}
	}
}
