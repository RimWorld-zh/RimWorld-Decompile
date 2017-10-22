using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_RearmTraps : WorkGiver_Scanner
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
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.RearmTrap).GetEnumerator())
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

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RearmTrap) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = t.Position.GetThingList(t.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						IntVec3 intVec = default(IntVec3);
						if (thingList[i] != t && thingList[i].def.category == ThingCategory.Item && (thingList[i].IsForbidden(pawn) || thingList[i].IsInValidStorage() || !HaulAIUtility.CanHaulAside(pawn, thingList[i], out intVec)))
						{
							goto IL_00c7;
						}
					}
					result = true;
				}
			}
			goto IL_00ec;
			IL_00c7:
			result = false;
			goto IL_00ec;
			IL_00ec:
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			List<Thing> thingList = t.Position.GetThingList(t.Map);
			int num = 0;
			Job result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (thingList[num] != t && thingList[num].def.category == ThingCategory.Item)
					{
						Job job = HaulAIUtility.HaulAsideJobFor(pawn, thingList[num]);
						if (job != null)
						{
							result = job;
							break;
						}
					}
					num++;
					continue;
				}
				result = new Job(JobDefOf.RearmTrap, t);
				break;
			}
			return result;
		}
	}
}
