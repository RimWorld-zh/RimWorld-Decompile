using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021D RID: 541
	public class ThoughtWorker_Imprisoned : ThoughtWorker
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x00059350 File Offset: 0x00057750
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.IsPrisoner;
		}
	}
}
