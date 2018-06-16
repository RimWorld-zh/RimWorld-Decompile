using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000162 RID: 354
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00049174 File Offset: 0x00047574
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
