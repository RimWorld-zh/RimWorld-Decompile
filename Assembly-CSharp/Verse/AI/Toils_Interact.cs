using System;

namespace Verse.AI
{
	// Token: 0x02000A4F RID: 2639
	internal class Toils_Interact
	{
		// Token: 0x06003ABF RID: 15039 RVA: 0x001F25F0 File Offset: 0x001F09F0
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
