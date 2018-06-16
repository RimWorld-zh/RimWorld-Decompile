using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000738 RID: 1848
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x060028BF RID: 10431 RVA: 0x0015B6C0 File Offset: 0x00159AC0
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}

		// Token: 0x0400164A RID: 5706
		public ThingDef thingToSpawn;

		// Token: 0x0400164B RID: 5707
		public int spawnCount = 1;

		// Token: 0x0400164C RID: 5708
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x0400164D RID: 5709
		public int spawnMaxAdjacent = -1;

		// Token: 0x0400164E RID: 5710
		public bool spawnForbidden;

		// Token: 0x0400164F RID: 5711
		public bool requiresPower;

		// Token: 0x04001650 RID: 5712
		public bool writeTimeLeftToSpawn;

		// Token: 0x04001651 RID: 5713
		public bool showMessageIfOwned;

		// Token: 0x04001652 RID: 5714
		public string saveKeysPrefix;
	}
}
