using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000758 RID: 1880
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		// Token: 0x06002993 RID: 10643 RVA: 0x00161A34 File Offset: 0x0015FE34
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
