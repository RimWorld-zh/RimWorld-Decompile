using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_TendSelf.<PotentialWorkThingsGlobal>c__Iterator66 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_TendSelf.<PotentialWorkThingsGlobal>c__Iterator66();
			<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
			<PotentialWorkThingsGlobal>c__Iterator.<$>pawn = pawn;
			WorkGiver_TendSelf.<PotentialWorkThingsGlobal>c__Iterator66 expr_15 = <PotentialWorkThingsGlobal>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool flag = base.HasJobOnThing(pawn, t, forced) && pawn == t && pawn.playerSettings != null;
			if (flag && !pawn.playerSettings.selfTend)
			{
				JobFailReason.Is("SelfTendDisabled".Translate());
			}
			return flag && pawn.playerSettings.selfTend;
		}
	}
}
