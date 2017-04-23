using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		private static List<IntVec3> removedRoofs = new List<IntVec3>();

		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveRoof.<MakeNewToils>c__Iterator12 <MakeNewToils>c__Iterator = new JobDriver_RemoveRoof.<MakeNewToils>c__Iterator12();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_RemoveRoof.<MakeNewToils>c__Iterator12 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}
	}
}
