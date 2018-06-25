using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000213 RID: 531
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x00058E14 File Offset: 0x00057214
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
