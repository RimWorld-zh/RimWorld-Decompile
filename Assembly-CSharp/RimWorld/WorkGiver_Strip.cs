using System.Collections.Generic;
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

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Strip).GetEnumerator())
			{
				Designation des;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						des = enumerator.Current;
						if (des.target.HasThing)
							break;
						Log.ErrorOnce("Strip designation has no target.", 63126);
						continue;
					}
					yield break;
				}
				yield return des.target.Thing;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_0100:
			/*Error near IL_0101: Unexpected return in MoveNext()*/;
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				result = ((byte)(pawn.CanReserve(target, 1, -1, null, forced) ? (StrippableUtility.CanBeStrippedByColony(t) ? 1 : 0) : 0) != 0);
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Strip, t);
		}
	}
}
