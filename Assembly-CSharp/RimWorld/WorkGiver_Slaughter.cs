using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Slaughter.<PotentialWorkThingsGlobal>c__Iterator5A <PotentialWorkThingsGlobal>c__Iterator5A = new WorkGiver_Slaughter.<PotentialWorkThingsGlobal>c__Iterator5A();
			<PotentialWorkThingsGlobal>c__Iterator5A.pawn = pawn;
			<PotentialWorkThingsGlobal>c__Iterator5A.<$>pawn = pawn;
			WorkGiver_Slaughter.<PotentialWorkThingsGlobal>c__Iterator5A expr_15 = <PotentialWorkThingsGlobal>c__Iterator5A;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter) == null)
			{
				return false;
			}
			if (pawn2.InAggroMentalState)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, ReservationLayer.Default, forced))
			{
				return false;
			}
			if (pawn.story != null && pawn.story.DisabledWorkTags.Contains(WorkTags.Violent))
			{
				JobFailReason.Is("IsIncapableOfViolenceShort".Translate());
				return false;
			}
			return true;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Slaughter, t);
		}
	}
}
