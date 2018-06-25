using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		public RecordWorker_TimeDrafted()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
