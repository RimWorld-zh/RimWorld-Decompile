using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000215 RID: 533
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x060009F6 RID: 2550 RVA: 0x00058F00 File Offset: 0x00057300
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
