using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000164 RID: 356
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x000491D4 File Offset: 0x000475D4
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
			yield break;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00049200 File Offset: 0x00047600
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0004921C File Offset: 0x0004761C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = pawn == t && pawn.playerSettings != null && base.HasJobOnThing(pawn, t, forced);
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate(), null);
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
