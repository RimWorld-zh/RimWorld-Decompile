using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A5 RID: 1189
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x0600154C RID: 5452 RVA: 0x000BD754 File Offset: 0x000BBB54
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
