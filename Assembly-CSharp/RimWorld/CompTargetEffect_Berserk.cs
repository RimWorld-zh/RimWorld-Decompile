using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000751 RID: 1873
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06002986 RID: 10630 RVA: 0x00161524 File Offset: 0x0015F924
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead)
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true, false, null, false);
			}
		}
	}
}
