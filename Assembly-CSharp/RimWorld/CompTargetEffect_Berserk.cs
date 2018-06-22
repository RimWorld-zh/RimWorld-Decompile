using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074F RID: 1871
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06002982 RID: 10626 RVA: 0x001613D4 File Offset: 0x0015F7D4
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
