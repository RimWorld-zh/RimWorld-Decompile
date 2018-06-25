using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000A4B RID: 2635
	public static class Toils_Effects
	{
		// Token: 0x06003AB5 RID: 15029 RVA: 0x001F2538 File Offset: 0x001F0938
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
