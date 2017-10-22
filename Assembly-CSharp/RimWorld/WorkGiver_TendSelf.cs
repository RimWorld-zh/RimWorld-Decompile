using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_TendSelf : WorkGiver_Tend
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			yield return (Thing)pawn;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = pawn == t && pawn.playerSettings != null && base.HasJobOnThing(pawn, t, forced);
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate());
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
