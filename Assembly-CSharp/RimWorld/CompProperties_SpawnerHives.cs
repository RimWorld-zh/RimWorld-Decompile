using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000739 RID: 1849
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x0400165E RID: 5726
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x0400165F RID: 5727
		public float HiveSpawnRadius = 10f;

		// Token: 0x04001660 RID: 5728
		public FloatRange HiveSpawnIntervalDays = new FloatRange(1.6f, 2.1f);

		// Token: 0x060028E0 RID: 10464 RVA: 0x0015CC9C File Offset: 0x0015B09C
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}
	}
}
