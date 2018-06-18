using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000753 RID: 1875
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x001611FC File Offset: 0x0015F5FC
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
