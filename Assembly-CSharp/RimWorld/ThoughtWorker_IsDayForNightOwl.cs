using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000220 RID: 544
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x0005940C File Offset: 0x0005780C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
