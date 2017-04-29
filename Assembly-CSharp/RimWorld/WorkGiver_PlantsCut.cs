using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		[DebuggerHidden]
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_PlantsCut.<PotentialWorkThingsGlobal>c__Iterator63 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_PlantsCut.<PotentialWorkThingsGlobal>c__Iterator63();
			<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
			<PotentialWorkThingsGlobal>c__Iterator.<$>pawn = pawn;
			WorkGiver_PlantsCut.<PotentialWorkThingsGlobal>c__Iterator63 expr_15 = <PotentialWorkThingsGlobal>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.category != ThingCategory.Plant)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, false))
			{
				return null;
			}
			if (t.IsForbidden(pawn))
			{
				return null;
			}
			if (t.IsBurning())
			{
				return null;
			}
			foreach (Designation current in pawn.Map.designationManager.AllDesignationsOn(t))
			{
				if (current.def == DesignationDefOf.HarvestPlant)
				{
					Job result;
					if (!((Plant)t).HarvestableNow)
					{
						result = null;
						return result;
					}
					result = new Job(JobDefOf.Harvest, t);
					return result;
				}
				else if (current.def == DesignationDefOf.CutPlant)
				{
					Job result = new Job(JobDefOf.CutPlant, t);
					return result;
				}
			}
			return null;
		}
	}
}
