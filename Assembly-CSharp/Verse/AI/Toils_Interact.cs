using System;

namespace Verse.AI
{
	// Token: 0x02000A4D RID: 2637
	internal class Toils_Interact
	{
		// Token: 0x06003AC0 RID: 15040 RVA: 0x001F2AE8 File Offset: 0x001F0EE8
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
	}
}
