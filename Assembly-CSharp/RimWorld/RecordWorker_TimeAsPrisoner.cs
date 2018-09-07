using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		public RecordWorker_TimeAsPrisoner()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
