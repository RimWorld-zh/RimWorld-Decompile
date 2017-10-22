using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.IsTeetotaler())
			{
				result = false;
			}
			else if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				result = false;
			}
			else
			{
				Hediff firstHediffOfDef = other.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
				result = ((firstHediffOfDef != null && firstHediffOfDef.Visible) ? true : false);
			}
			return result;
		}
	}
}
