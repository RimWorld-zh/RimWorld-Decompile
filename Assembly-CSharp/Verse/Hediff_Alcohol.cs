using RimWorld;

namespace Verse
{
	public class Hediff_Alcohol : HediffWithComps
	{
		private const int HangoverCheckInterval = 300;

		public override void Tick()
		{
			base.Tick();
			if (this.CurStageIndex >= 3 && base.pawn.IsHashIntervalTick(300) && this.HangoverSusceptible(base.pawn))
			{
				Hediff firstHediffOfDef = base.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity = 1f;
				}
				else
				{
					firstHediffOfDef = HediffMaker.MakeHediff(HediffDefOf.Hangover, base.pawn, null);
					firstHediffOfDef.Severity = 1f;
					base.pawn.health.AddHediff(firstHediffOfDef, null, default(DamageInfo?));
				}
			}
		}

		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}
	}
}
