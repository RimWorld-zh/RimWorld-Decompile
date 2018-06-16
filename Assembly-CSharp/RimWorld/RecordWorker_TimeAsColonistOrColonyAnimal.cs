using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A5 RID: 1189
	public class RecordWorker_TimeAsColonistOrColonyAnimal : RecordWorker
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x000BD59C File Offset: 0x000BB99C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
