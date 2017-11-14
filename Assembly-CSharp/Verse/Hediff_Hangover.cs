using RimWorld;

namespace Verse
{
	public class Hediff_Hangover : HediffWithComps
	{
		public override bool Visible
		{
			get
			{
				if (base.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false))
				{
					return false;
				}
				return base.Visible;
			}
		}
	}
}
