using System;
using Verse;

namespace RimWorld
{
	public class JobDriver_Milk : JobDriver_GatherAnimalBodyResources
	{
		public JobDriver_Milk()
		{
		}

		protected override float WorkTotal
		{
			get
			{
				return 400f;
			}
		}

		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMilkable>();
		}
	}
}
