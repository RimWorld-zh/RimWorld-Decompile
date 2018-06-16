using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073D RID: 1853
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x060028E5 RID: 10469 RVA: 0x0015CA30 File Offset: 0x0015AE30
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}

		// Token: 0x04001660 RID: 5728
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x04001661 RID: 5729
		public float HiveSpawnRadius = 10f;

		// Token: 0x04001662 RID: 5730
		public FloatRange HiveSpawnIntervalDays = new FloatRange(1.6f, 2.1f);
	}
}
