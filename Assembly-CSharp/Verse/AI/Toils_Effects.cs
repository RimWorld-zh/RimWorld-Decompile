using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000A49 RID: 2633
	public static class Toils_Effects
	{
		// Token: 0x06003AB1 RID: 15025 RVA: 0x001F240C File Offset: 0x001F080C
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
