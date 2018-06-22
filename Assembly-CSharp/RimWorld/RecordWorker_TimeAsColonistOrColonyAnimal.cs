using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A1 RID: 1185
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x06001544 RID: 5444 RVA: 0x000BD5B4 File Offset: 0x000BB9B4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
