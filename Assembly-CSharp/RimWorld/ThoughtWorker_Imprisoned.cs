using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021B RID: 539
	public class ThoughtWorker_Imprisoned : ThoughtWorker
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x0005918C File Offset: 0x0005758C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.IsPrisoner;
		}
	}
}
