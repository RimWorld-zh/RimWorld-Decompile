using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	public static class Toils_Goto
	{
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode);
		}

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

		public static Toil MoveOffTargetBlueprint(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(targetInd).Thing as Blueprint;
				if (thing == null || !actor.Position.IsInside(thing))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		[CompilerGenerated]
		private sealed class <GotoThing>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal PathEndMode peMode;

			public <GotoThing>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(this.ind), this.peMode);
			}
		}

		[CompilerGenerated]
		private sealed class <GotoThing>c__AnonStorey1
		{
			internal Toil toil;

			internal IntVec3 exactCell;

			public <GotoThing>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				actor.pather.StartPath(this.exactCell, PathEndMode.OnCell);
			}
		}

		[CompilerGenerated]
		private sealed class <GotoCell>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal PathEndMode peMode;

			public <GotoCell>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(this.ind), this.peMode);
			}
		}

		[CompilerGenerated]
		private sealed class <GotoCell>c__AnonStorey3
		{
			internal Toil toil;

			internal IntVec3 cell;

			internal PathEndMode peMode;

			public <GotoCell>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				actor.pather.StartPath(this.cell, this.peMode);
			}
		}

		[CompilerGenerated]
		private sealed class <MoveOffTargetBlueprint>c__AnonStorey4
		{
			internal Toil toil;

			internal TargetIndex targetInd;

			public <MoveOffTargetBlueprint>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(this.targetInd).Thing as Blueprint;
				if (thing == null || !actor.Position.IsInside(thing))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
			}
		}
	}
}
