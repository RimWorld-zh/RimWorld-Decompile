using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		protected override void Init()
		{
			if (base.Plant.def.plant.harvestedThingDef != null && base.Plant.YieldNow() > 0)
			{
				base.xpPerTick = 0.11f;
			}
			else
			{
				base.xpPerTick = 0f;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil item in base.MakeNewToils())
			{
				yield return item;
			}
			yield return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
