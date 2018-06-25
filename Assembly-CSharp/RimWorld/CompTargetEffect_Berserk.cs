using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000751 RID: 1873
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06002985 RID: 10629 RVA: 0x00161784 File Offset: 0x0015FB84
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
