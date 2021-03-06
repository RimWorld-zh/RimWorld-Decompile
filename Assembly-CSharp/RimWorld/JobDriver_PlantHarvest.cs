﻿using System;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		public JobDriver_PlantHarvest()
		{
		}

		protected override void Init()
		{
			this.xpPerTick = 0.085f;
		}

		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
