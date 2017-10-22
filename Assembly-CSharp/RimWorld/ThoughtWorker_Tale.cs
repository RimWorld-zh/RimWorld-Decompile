using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return other.RaceProps.Humanlike ? (RelationsUtility.PawnsKnowEachOther(p, other) ? ((Find.TaleManager.GetLatestTale(base.def.taleDef, other) != null) ? true : false) : false) : false;
		}
	}
}
