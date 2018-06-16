using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000215 RID: 533
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x060009F8 RID: 2552 RVA: 0x00058EBC File Offset: 0x000572BC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
