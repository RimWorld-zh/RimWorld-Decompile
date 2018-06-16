using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000220 RID: 544
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x000593C8 File Offset: 0x000577C8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
