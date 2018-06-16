using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000753 RID: 1875
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06002987 RID: 10631 RVA: 0x00161168 File Offset: 0x0015F568
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
