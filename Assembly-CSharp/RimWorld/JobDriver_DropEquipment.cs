using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_DropEquipment : JobDriver
	{
		private const int DurationTicks = 30;

		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DropEquipment.<MakeNewToils>c__Iterator52 <MakeNewToils>c__Iterator = new JobDriver_DropEquipment.<MakeNewToils>c__Iterator52();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_DropEquipment.<MakeNewToils>c__Iterator52 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
