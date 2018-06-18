using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AE RID: 1198
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x0600155F RID: 5471 RVA: 0x000BD788 File Offset: 0x000BBB88
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
