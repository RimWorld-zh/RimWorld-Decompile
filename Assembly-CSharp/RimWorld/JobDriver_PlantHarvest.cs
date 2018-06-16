using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200008D RID: 141
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x0600039D RID: 925 RVA: 0x000290F3 File Offset: 0x000274F3
		protected override void Init()
		{
			this.xpPerTick = 0.0935f;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00029104 File Offset: 0x00027504
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
