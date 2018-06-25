using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000750 RID: 1872
	public abstract class CompTargetEffect : ThingComp
	{
		// Token: 0x06002984 RID: 10628
		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
