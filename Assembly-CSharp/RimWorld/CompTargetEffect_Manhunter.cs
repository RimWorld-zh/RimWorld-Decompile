using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000758 RID: 1880
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x06002991 RID: 10641 RVA: 0x0016135C File Offset: 0x0015F75C
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
