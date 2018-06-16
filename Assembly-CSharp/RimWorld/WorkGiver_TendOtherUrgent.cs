using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000163 RID: 355
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x000491AC File Offset: 0x000475AC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
