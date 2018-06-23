using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000737 RID: 1847
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x04001654 RID: 5716
		public ThingDef filthDef = null;

		// Token: 0x04001655 RID: 5717
		public int spawnCountOnSpawn = 5;

		// Token: 0x04001656 RID: 5718
		public float spawnMtbHours = 12f;

		// Token: 0x04001657 RID: 5719
		public float spawnRadius = 3f;

		// Token: 0x04001658 RID: 5720
		public float spawnEveryDays = -1f;

		// Token: 0x04001659 RID: 5721
		public RotStage? requiredRotStage;

		// Token: 0x060028D2 RID: 10450 RVA: 0x0015C3D0 File Offset: 0x0015A7D0
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}
	}
}
