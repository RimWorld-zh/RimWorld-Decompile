using System.Collections.Generic;
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

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			for (int i = 0; i < desList.Count; i++)
			{
				Designation des = desList[i];
				if (des.def == DesignationDefOf.CutPlant || des.def == DesignationDefOf.HarvestPlant)
				{
					yield return des.target.Thing;
				}
			}
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
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.AllDesignationsOn(t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Designation current = enumerator.Current;
					if (current.def == DesignationDefOf.HarvestPlant)
						goto IL_0078;
					if (current.def == DesignationDefOf.CutPlant)
					{
						return new Job(JobDefOf.CutPlant, t);
					}
				}
				goto end_IL_005c;
				IL_0078:
				if (!((Plant)t).HarvestableNow)
				{
					return null;
				}
				return new Job(JobDefOf.Harvest, t);
				end_IL_005c:;
			}
			return null;
		}
	}
}
