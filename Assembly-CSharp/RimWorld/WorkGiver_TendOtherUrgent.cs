using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000163 RID: 355
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x00049194 File Offset: 0x00047594
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
