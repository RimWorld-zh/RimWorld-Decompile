using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		public RecordWorker_TimeUnderRoof()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
