using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A4 RID: 1188
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x000BD628 File Offset: 0x000BBA28
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
