using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000217 RID: 535
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x060009F9 RID: 2553 RVA: 0x0005907C File Offset: 0x0005747C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
