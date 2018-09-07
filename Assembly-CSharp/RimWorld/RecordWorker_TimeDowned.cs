using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeDowned : RecordWorker
	{
		public RecordWorker_TimeDowned()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
