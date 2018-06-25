using System;

namespace Verse.AI
{
	// Token: 0x02000A4E RID: 2638
	internal class Toils_Interact
	{
		// Token: 0x06003AC1 RID: 15041 RVA: 0x001F2E14 File Offset: 0x001F1214
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
