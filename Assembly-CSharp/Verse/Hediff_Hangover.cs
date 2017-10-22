using RimWorld;

namespace Verse
{
	public class Hediff_Hangover : HediffWithComps
	{
		public override bool Visible
		{
			get
			{
				return !base.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
