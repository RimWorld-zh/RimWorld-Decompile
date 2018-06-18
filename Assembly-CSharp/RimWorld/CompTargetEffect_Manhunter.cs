using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000758 RID: 1880
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x06002993 RID: 10643 RVA: 0x001613F0 File Offset: 0x0015F7F0
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
