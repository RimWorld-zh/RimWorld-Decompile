using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021A RID: 538
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0005915C File Offset: 0x0005755C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
