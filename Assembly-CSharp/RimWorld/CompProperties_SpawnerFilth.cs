using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073B RID: 1851
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x060028D7 RID: 10455 RVA: 0x0015C164 File Offset: 0x0015A564
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}

		// Token: 0x04001656 RID: 5718
		public ThingDef filthDef = null;

		// Token: 0x04001657 RID: 5719
		public int spawnCountOnSpawn = 5;

		// Token: 0x04001658 RID: 5720
		public float spawnMtbHours = 12f;

		// Token: 0x04001659 RID: 5721
		public float spawnRadius = 3f;

		// Token: 0x0400165A RID: 5722
		public float spawnEveryDays = -1f;

		// Token: 0x0400165B RID: 5723
		public RotStage? requiredRotStage;
	}
}
