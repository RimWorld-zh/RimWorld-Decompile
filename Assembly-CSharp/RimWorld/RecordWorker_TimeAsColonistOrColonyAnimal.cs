using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A3 RID: 1187
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x06001547 RID: 5447 RVA: 0x000BD904 File Offset: 0x000BBD04
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
