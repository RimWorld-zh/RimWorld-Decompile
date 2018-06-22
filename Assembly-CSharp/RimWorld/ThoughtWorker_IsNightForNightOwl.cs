using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000221 RID: 545
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x00059458 File Offset: 0x00057858
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
