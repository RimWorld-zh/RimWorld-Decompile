using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021D RID: 541
	public class ThoughtWorker_Imprisoned : ThoughtWorker
	{
		// Token: 0x06000A05 RID: 2565 RVA: 0x0005934C File Offset: 0x0005774C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.IsPrisoner;
		}
	}
}
