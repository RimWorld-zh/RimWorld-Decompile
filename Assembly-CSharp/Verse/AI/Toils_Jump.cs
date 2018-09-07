using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public static class Toils_Jump
	{
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		public static Toil JumpIf(Toil jumpTarget, Func<bool> condition)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (condition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
				}
			};
			return toil;
		}

		public static Toil JumpIfTargetDespawnedOrNull(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		public static Toil JumpIfTargetInvalid(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				if (!toil.actor.jobs.curJob.GetTarget(ind).IsValid)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		public static Toil JumpIfTargetNotHittable(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		public static Toil JumpIfTargetDowned(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Pawn pawn = curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		public static Toil JumpIfHaveTargetInQueue(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		public static Toil JumpIfCannotTouch(TargetIndex ind, PathEndMode peMode, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (!actor.CanReachImmediate(target, peMode))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		[CompilerGenerated]
		private sealed class <Jump>c__AnonStorey0
		{
			internal Toil toil;

			internal Toil jumpTarget;

			public <Jump>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.jobs.curDriver.JumpToToil(this.jumpTarget);
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIf>c__AnonStorey1
		{
			internal Func<bool> condition;

			internal Toil toil;

			internal Toil jumpTarget;

			public <JumpIf>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				if (this.condition())
				{
					this.toil.actor.jobs.curDriver.JumpToToil(this.jumpTarget);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfTargetDespawnedOrNull>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Toil jumpToil;

			public <JumpIfTargetDespawnedOrNull>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Thing thing = this.toil.actor.jobs.curJob.GetTarget(this.ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					this.toil.actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfTargetInvalid>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Toil jumpToil;

			public <JumpIfTargetInvalid>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				if (!this.toil.actor.jobs.curJob.GetTarget(this.ind).IsValid)
				{
					this.toil.actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfTargetNotHittable>c__AnonStorey4
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Toil jumpToil;

			public <JumpIfTargetNotHittable>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(this.ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfTargetDowned>c__AnonStorey5
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Toil jumpToil;

			public <JumpIfTargetDowned>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Pawn pawn = curJob.GetTarget(this.ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfHaveTargetInQueue>c__AnonStorey6
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Toil jumpToil;

			public <JumpIfHaveTargetInQueue>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(this.ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfCannotTouch>c__AnonStorey7
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal PathEndMode peMode;

			internal Toil jumpToil;

			public <JumpIfCannotTouch>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(this.ind);
				if (!actor.CanReachImmediate(target, this.peMode))
				{
					actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}
	}
}
