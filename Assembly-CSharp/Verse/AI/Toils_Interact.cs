using System;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	internal class Toils_Interact
	{
		public Toils_Interact()
		{
		}

		public static Toil DestroyThing(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (!thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			};
			return toil;
		}

		[CompilerGenerated]
		private sealed class <DestroyThing>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <DestroyThing>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(this.ind).Thing;
				if (!thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}
	}
}
