using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A7 RID: 1191
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x06001551 RID: 5457 RVA: 0x000BD608 File Offset: 0x000BBA08
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
