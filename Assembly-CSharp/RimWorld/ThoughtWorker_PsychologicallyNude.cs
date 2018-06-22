using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021A RID: 538
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06000A00 RID: 2560 RVA: 0x000591A0 File Offset: 0x000575A0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
