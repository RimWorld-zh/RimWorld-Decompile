using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A5 RID: 1189
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x0600154B RID: 5451 RVA: 0x000BD954 File Offset: 0x000BBD54
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
