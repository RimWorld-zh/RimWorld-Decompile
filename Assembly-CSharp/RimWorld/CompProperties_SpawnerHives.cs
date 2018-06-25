using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073B RID: 1851
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x04001662 RID: 5730
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x04001663 RID: 5731
		public float HiveSpawnRadius = 10f;

		// Token: 0x04001664 RID: 5732
		public FloatRange HiveSpawnIntervalDays = new FloatRange(1.6f, 2.1f);

		// Token: 0x060028E3 RID: 10467 RVA: 0x0015D04C File Offset: 0x0015B44C
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}
	}
}
