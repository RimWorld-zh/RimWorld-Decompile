using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		private static string NoPathTrans;

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

		public static void Reset()
		{
			WorkGiver_Miner.NoPathTrans = "NoPath".Translate();
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine).GetEnumerator())
			{
				Mineable i;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						Designation des = enumerator.Current;
						bool mayBeAccessible = false;
						for (int j = 0; j < 8; j++)
						{
							IntVec3 c = des.target.Cell + GenAdj.AdjacentCells[j];
							if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
							{
								mayBeAccessible = true;
								break;
							}
						}
						if (mayBeAccessible)
						{
							i = des.target.Cell.GetFirstMineable(pawn.Map);
							if (i != null)
								break;
						}
						continue;
					}
					yield break;
				}
				yield return (Thing)i;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_0187:
			/*Error near IL_0188: Unexpected return in MoveNext()*/;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			Thing thing;
			if (!t.def.mineable)
			{
				result = null;
			}
			else if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null)
			{
				result = null;
			}
			else if (!pawn.CanReserve(t, 1, -1, null, false))
			{
				result = null;
			}
			else
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
						if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.Walkable(t.Map) && !intVec2.Standable(t.Map))
						{
							thing = null;
							List<Thing> thingList = intVec2.GetThingList(t.Map);
							int num = 0;
							while (num < thingList.Count)
							{
								if (!thingList[num].def.designateHaulable || thingList[num].def.passability != Traversability.PassThroughOnly)
								{
									num++;
									continue;
								}
								thing = thingList[num];
								break;
							}
							if (thing != null)
								goto IL_01d2;
						}
					}
					JobFailReason.Is(WorkGiver_Miner.NoPathTrans);
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Mine, t, 1500, true);
				}
			}
			goto IL_023b;
			IL_01d2:
			Job job = HaulAIUtility.HaulAsideJobFor(pawn, thing);
			if (job != null)
			{
				result = job;
			}
			else
			{
				JobFailReason.Is(WorkGiver_Miner.NoPathTrans);
				result = null;
			}
			goto IL_023b;
			IL_023b:
			return result;
		}
	}
}
