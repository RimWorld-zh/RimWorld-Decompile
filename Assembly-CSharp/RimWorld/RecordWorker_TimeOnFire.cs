using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AC RID: 1196
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x0600155A RID: 5466 RVA: 0x000BD8D4 File Offset: 0x000BBCD4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
