using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		protected override void Init()
		{
			if (base.Plant.def.plant.harvestedThingDef != null && base.Plant.YieldNow() > 0)
			{
				this.xpPerTick = 0.11f;
			}
			else
			{
				this.xpPerTick = 0f;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantCut.<MakeNewToils>c__Iterator48 <MakeNewToils>c__Iterator = new JobDriver_PlantCut.<MakeNewToils>c__Iterator48();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_PlantCut.<MakeNewToils>c__Iterator48 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
