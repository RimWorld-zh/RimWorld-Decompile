using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		protected override void Init()
		{
			base.xpPerTick = 0.11f;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil item in base.MakeNewToils())
			{
				yield return item;
			}
			yield return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
