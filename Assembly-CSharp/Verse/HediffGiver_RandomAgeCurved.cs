using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		public SimpleCurve ageFractionMtbDaysCurve = null;

		public int minPlayerPopulation = 0;

		public HediffGiver_RandomAgeCurved()
		{
		}

		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (this.minPlayerPopulation <= 0 || pawn.Faction != Faction.OfPlayer || PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.Count<Pawn>() >= this.minPlayerPopulation)
				{
					if (base.TryApply(pawn, null))
					{
						base.SendLetter(pawn, cause);
					}
				}
			}
		}
	}
}
