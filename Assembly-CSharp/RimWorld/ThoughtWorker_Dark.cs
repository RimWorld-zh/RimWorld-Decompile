using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000211 RID: 529
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x060009F0 RID: 2544 RVA: 0x00058C50 File Offset: 0x00057050
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
