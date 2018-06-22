using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AA RID: 1194
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x06001556 RID: 5462 RVA: 0x000BD784 File Offset: 0x000BBB84
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
