using Verse;

namespace RimWorld
{
	public class CompTargetEffect_PsychicShock : CompTargetEffect
	{
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead)
			{
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn, null);
				BodyPartRecord part = null;
				pawn.RaceProps.body.GetPartsWithTag("ConsciousnessSource").TryRandomElement<BodyPartRecord>(out part);
				pawn.health.AddHediff(hediff, part, null);
			}
		}
	}
}
