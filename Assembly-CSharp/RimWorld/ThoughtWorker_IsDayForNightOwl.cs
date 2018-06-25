using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000222 RID: 546
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x0005958C File Offset: 0x0005798C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
