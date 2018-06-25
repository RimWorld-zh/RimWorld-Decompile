using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021C RID: 540
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06000A03 RID: 2563 RVA: 0x0005931C File Offset: 0x0005771C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
