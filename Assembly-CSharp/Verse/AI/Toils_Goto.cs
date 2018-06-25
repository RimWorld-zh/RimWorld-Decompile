using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A50 RID: 2640
	public static class Toils_Goto
	{
		// Token: 0x06003AD1 RID: 15057 RVA: 0x001F3AE8 File Offset: 0x001F1EE8
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode);
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x001F3B04 File Offset: 0x001F1F04
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

		// Token: 0x06003AD3 RID: 15059 RVA: 0x001F3B74 File Offset: 0x001F1F74
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

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001F3BD8 File Offset: 0x001F1FD8
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

		// Token: 0x06003AD5 RID: 15061 RVA: 0x001F3C38 File Offset: 0x001F2038
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

		// Token: 0x06003AD6 RID: 15062 RVA: 0x001F3C98 File Offset: 0x001F2098
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
