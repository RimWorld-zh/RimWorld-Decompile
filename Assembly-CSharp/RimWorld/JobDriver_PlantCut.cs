using System;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		public JobDriver_PlantCut()
		{
		}

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

		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
