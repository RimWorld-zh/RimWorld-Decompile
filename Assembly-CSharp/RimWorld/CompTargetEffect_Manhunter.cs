using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000756 RID: 1878
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x06002990 RID: 10640 RVA: 0x00161718 File Offset: 0x0015FB18
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
