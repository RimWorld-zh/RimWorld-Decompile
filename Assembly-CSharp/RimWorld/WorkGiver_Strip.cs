using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Strip.<PotentialWorkThingsGlobal>c__Iterator65 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_Strip.<PotentialWorkThingsGlobal>c__Iterator65();
			<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
			<PotentialWorkThingsGlobal>c__Iterator.<$>pawn = pawn;
			WorkGiver_Strip.<PotentialWorkThingsGlobal>c__Iterator65 expr_15 = <PotentialWorkThingsGlobal>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) != null && pawn.CanReserve(t, 1, -1, ReservationLayer.Default, forced) && StrippableUtility.CanBeStrippedByColony(t);
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Strip, t);
		}
	}
}
