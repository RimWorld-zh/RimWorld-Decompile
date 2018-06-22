using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4E RID: 2638
	public static class Toils_Goto
	{
		// Token: 0x06003ACD RID: 15053 RVA: 0x001F39BC File Offset: 0x001F1DBC
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode);
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x001F39D8 File Offset: 0x001F1DD8
		public static Toil GotoThing(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x001F3A48 File Offset: 0x001F1E48
		public static Toil GotoThing(TargetIndex ind, IntVec3 exactCell)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(exactCell, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x001F3AAC File Offset: 0x001F1EAC
		public static Toil GotoCell(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x001F3B0C File Offset: 0x001F1F0C
		public static Toil GotoCell(IntVec3 cell, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(cell, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x001F3B6C File Offset: 0x001F1F6C
		public static Toil MoveOffTargetBlueprint(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(targetInd).Thing as Blueprint;
				IntVec3 c;
				if (thing == null || !actor.Position.IsInside(thing))
				{
					actor.jobs.curDriver.ReadyForNextToil();
				}
				else if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
				}
				else
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}
	}
}
