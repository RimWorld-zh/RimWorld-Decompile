using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000217 RID: 535
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x060009FA RID: 2554 RVA: 0x00059080 File Offset: 0x00057480
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
