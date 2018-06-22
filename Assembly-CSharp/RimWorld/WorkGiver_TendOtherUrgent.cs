using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000163 RID: 355
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x00049198 File Offset: 0x00047598
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
