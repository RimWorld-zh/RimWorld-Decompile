using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			int i = 0;
			while (true)
			{
				if (i < desList.Count)
				{
					if (desList[i].def != DesignationDefOf.Flick)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return desList[i].target.Thing;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				result = ((byte)(pawn.CanReserve(target, 1, -1, null, forced) ? 1 : 0) != 0);
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Flick, t);
		}
	}
}
