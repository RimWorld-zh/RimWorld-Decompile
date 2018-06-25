using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000739 RID: 1849
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x04001658 RID: 5720
		public ThingDef filthDef = null;

		// Token: 0x04001659 RID: 5721
		public int spawnCountOnSpawn = 5;

		// Token: 0x0400165A RID: 5722
		public float spawnMtbHours = 12f;

		// Token: 0x0400165B RID: 5723
		public float spawnRadius = 3f;

		// Token: 0x0400165C RID: 5724
		public float spawnEveryDays = -1f;

		// Token: 0x0400165D RID: 5725
		public RotStage? requiredRotStage;

		// Token: 0x060028D5 RID: 10453 RVA: 0x0015C780 File Offset: 0x0015AB80
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}
	}
}
