using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000222 RID: 546
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A0F RID: 2575 RVA: 0x00059588 File Offset: 0x00057988
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
