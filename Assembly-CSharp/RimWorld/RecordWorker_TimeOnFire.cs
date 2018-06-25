using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AC RID: 1196
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x06001559 RID: 5465 RVA: 0x000BDAD4 File Offset: 0x000BBED4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
