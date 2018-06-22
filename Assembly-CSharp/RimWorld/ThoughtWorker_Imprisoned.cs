using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021B RID: 539
	public class ThoughtWorker_Imprisoned : ThoughtWorker
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x000591D0 File Offset: 0x000575D0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.IsPrisoner;
		}
	}
}
