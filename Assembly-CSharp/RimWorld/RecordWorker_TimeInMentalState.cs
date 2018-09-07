using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		public RecordWorker_TimeInMentalState()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
