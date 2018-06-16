using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000752 RID: 1874
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06002985 RID: 10629
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
