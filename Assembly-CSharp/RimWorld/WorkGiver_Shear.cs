using System;
using Verse;

namespace RimWorld
{
	public class WorkGiver_Shear : WorkGiver_GatherAnimalBodyResources
	{
		public WorkGiver_Shear()
		{
		}

		protected override JobDef JobDef
		{
			get
			{
				return JobDefOf.Shear;
			}
		}

		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
