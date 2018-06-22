using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074E RID: 1870
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06002980 RID: 10624
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
