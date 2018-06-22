using System;

namespace Verse.AI
{
	// Token: 0x02000A4B RID: 2635
	internal class Toils_Interact
	{
		// Token: 0x06003ABC RID: 15036 RVA: 0x001F29BC File Offset: 0x001F0DBC
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
