using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06002987 RID: 10631
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
