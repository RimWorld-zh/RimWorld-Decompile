using System;
using Verse;

namespace RimWorld
{
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		public CompTargetEffect_PsychicShock()
		{
		}

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
