using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000756 RID: 1878
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x0600298F RID: 10639 RVA: 0x00161978 File Offset: 0x0015FD78
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
