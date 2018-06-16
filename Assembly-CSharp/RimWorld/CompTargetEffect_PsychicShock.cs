using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075A RID: 1882
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x06002995 RID: 10645 RVA: 0x00161418 File Offset: 0x0015F818
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead)
			{
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn, null);
				BodyPartRecord part = null;
				pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource).TryRandomElement(out part);
				pawn.health.AddHediff(hediff, part, null, null);
			}
		}
	}
}
