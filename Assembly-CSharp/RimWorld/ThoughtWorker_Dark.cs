using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000213 RID: 531
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x00058E10 File Offset: 0x00057210
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
