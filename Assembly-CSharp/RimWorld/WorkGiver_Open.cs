using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Open).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Designation des = enumerator.Current;
					yield return des.target.Thing;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00d6:
			/*Error near IL_00d7: Unexpected return in MoveNext()*/;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) == null)
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
			return new Job(JobDefOf.Open, t);
		}
	}
}
