using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		public ThoughtWorker_Tale()
		{
		}

		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				result = false;
			}
			else if (Find.TaleManager.GetLatestTale(this.def.taleDef, other) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
