using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000754 RID: 1876
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x0600298C RID: 10636 RVA: 0x001615C8 File Offset: 0x0015F9C8
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead)
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
			}
		}
	}
}
