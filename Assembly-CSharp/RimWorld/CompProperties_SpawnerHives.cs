using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073B RID: 1851
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x0400165E RID: 5726
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x0400165F RID: 5727
		public float HiveSpawnRadius = 10f;

		// Token: 0x04001660 RID: 5728
		public FloatRange HiveSpawnIntervalDays = new FloatRange(1.6f, 2.1f);

		// Token: 0x060028E4 RID: 10468 RVA: 0x0015CDEC File Offset: 0x0015B1EC
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}
	}
}
