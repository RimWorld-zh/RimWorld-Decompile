using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000A4D RID: 2637
	public static class Toils_Effects
	{
		// Token: 0x06003AB6 RID: 15030 RVA: 0x001F2114 File Offset: 0x001F0514
		public static Toil MakeSound(SoundDef soundDef)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return toil;
		}
	}
}
