using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000164 RID: 356
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x000491D0 File Offset: 0x000475D0
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return pawn;
			yield break;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x000491FC File Offset: 0x000475FC
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00049218 File Offset: 0x00047618
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
