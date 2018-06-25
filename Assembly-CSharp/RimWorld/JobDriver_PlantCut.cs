using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200008E RID: 142
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x00029124 File Offset: 0x00027524
		protected override void Init()
		{
			if (base.Plant.def.plant.harvestedThingDef != null && base.Plant.CanYieldNow())
			{
				this.xpPerTick = 0.0935f;
			}
			else
			{
				this.xpPerTick = 0f;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00029178 File Offset: 0x00027578
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
