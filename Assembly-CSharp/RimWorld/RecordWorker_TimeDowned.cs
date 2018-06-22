using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A3 RID: 1187
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x06001548 RID: 5448 RVA: 0x000BD604 File Offset: 0x000BBA04
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
