using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000162 RID: 354
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x0600074D RID: 1869 RVA: 0x0004915C File Offset: 0x0004755C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
