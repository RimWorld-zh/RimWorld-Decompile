using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000750 RID: 1872
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06002983 RID: 10627
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
