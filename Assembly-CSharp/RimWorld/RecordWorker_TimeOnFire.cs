using System;
using Verse;

namespace RimWorld
{
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		public RecordWorker_TimeOnFire()
		{
		}

		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
