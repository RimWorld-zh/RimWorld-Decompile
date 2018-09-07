using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeHauling : RecordWorker
	{
		public RecordWorker_TimeHauling()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
