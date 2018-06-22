using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000211 RID: 529
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x060009EE RID: 2542 RVA: 0x00058C94 File Offset: 0x00057094
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
