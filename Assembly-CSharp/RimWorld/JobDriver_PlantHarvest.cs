using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200008D RID: 141
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x0600039D RID: 925 RVA: 0x000290EB File Offset: 0x000274EB
		protected override void Init()
		{
			this.xpPerTick = 0.0935f;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000290FC File Offset: 0x000274FC
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
