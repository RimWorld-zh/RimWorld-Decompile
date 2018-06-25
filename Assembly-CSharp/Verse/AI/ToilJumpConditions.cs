using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	public static class ToilJumpConditions
	{
		public static Toil JumpIf(this Toil toil, Func<bool> jumpCondition, Toil jumpToil)
		{
			toil.AddPreTickAction(delegate
			{
				if (jumpCondition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			});
			return toil;
		}

		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		public static Toil JumpIfOutsideHomeArea(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return !toil.actor.Map.areaManager.Home[thing.Position];
			}, jumpToil);
		}

		[CompilerGenerated]
		private sealed class <JumpIf>c__AnonStorey0
		{
			internal Func<bool> jumpCondition;

			internal Toil toil;

			internal Toil jumpToil;

			public <JumpIf>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (this.jumpCondition())
				{
					this.toil.actor.jobs.curDriver.JumpToToil(this.jumpToil);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfDespawnedOrNull>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <JumpIfDespawnedOrNull>c__AnonStorey1()
			{
			}

			internal bool <>m__0()
			{
				Thing thing = this.toil.actor.jobs.curJob.GetTarget(this.ind).Thing;
				return thing == null || !thing.Spawned;
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfDespawnedOrNullOrForbidden>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <JumpIfDespawnedOrNullOrForbidden>c__AnonStorey2()
			{
			}

			internal bool <>m__0()
			{
				Thing thing = this.toil.actor.jobs.curJob.GetTarget(this.ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(this.toil.actor);
			}
		}

		[CompilerGenerated]
		private sealed class <JumpIfOutsideHomeArea>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <JumpIfOutsideHomeArea>c__AnonStorey3()
			{
			}

			internal bool <>m__0()
			{
				Thing thing = this.toil.actor.jobs.curJob.GetTarget(this.ind).Thing;
				return !this.toil.actor.Map.areaManager.Home[thing.Position];
			}
		}
	}
}
