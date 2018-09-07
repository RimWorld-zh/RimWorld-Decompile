using System;
using Verse;

namespace RimWorld
{
	public class JobDriver_PlantHarvest_Designated : JobDriver_PlantHarvest
	{
		public JobDriver_PlantHarvest_Designated()
		{
		}

		protected override DesignationDef RequiredDesignation
		{
			get
			{
				return DesignationDefOf.HarvestPlant;
			}
		}
	}
}
