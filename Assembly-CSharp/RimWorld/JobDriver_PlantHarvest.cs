using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		protected override void Init()
		{
			this.xpPerTick = 0.11f;
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantHarvest.<MakeNewToils>c__Iterator47 <MakeNewToils>c__Iterator = new JobDriver_PlantHarvest.<MakeNewToils>c__Iterator47();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_PlantHarvest.<MakeNewToils>c__Iterator47 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
