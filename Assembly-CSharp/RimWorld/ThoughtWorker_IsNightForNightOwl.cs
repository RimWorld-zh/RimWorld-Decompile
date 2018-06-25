using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000223 RID: 547
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x000595D4 File Offset: 0x000579D4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
