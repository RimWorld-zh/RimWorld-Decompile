using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000221 RID: 545
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x00059414 File Offset: 0x00057814
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
