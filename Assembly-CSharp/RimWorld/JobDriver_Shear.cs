using System;
using Verse;

namespace RimWorld
{
	public class JobDriver_Shear : JobDriver_GatherAnimalBodyResources
	{
		public JobDriver_Shear()
		{
		}

		protected override float WorkTotal
		{
			get
			{
				return 1700f;
			}
		}

		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompShearable>();
		}
	}
}
