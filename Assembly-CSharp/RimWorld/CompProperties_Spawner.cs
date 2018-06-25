using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000736 RID: 1846
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x04001648 RID: 5704
		public ThingDef thingToSpawn;

		// Token: 0x04001649 RID: 5705
		public int spawnCount = 1;

		// Token: 0x0400164A RID: 5706
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x0400164B RID: 5707
		public int spawnMaxAdjacent = -1;

		// Token: 0x0400164C RID: 5708
		public bool spawnForbidden;

		// Token: 0x0400164D RID: 5709
		public bool requiresPower;

		// Token: 0x0400164E RID: 5710
		public bool writeTimeLeftToSpawn;

		// Token: 0x0400164F RID: 5711
		public bool showMessageIfOwned;

		// Token: 0x04001650 RID: 5712
		public string saveKeysPrefix;

		// Token: 0x060028BE RID: 10430 RVA: 0x0015BA7C File Offset: 0x00159E7C
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}
	}
}
