using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021C RID: 540
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x00059320 File Offset: 0x00057720
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
