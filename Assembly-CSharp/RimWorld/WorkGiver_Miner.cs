using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000150 RID: 336
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		// Token: 0x0400032D RID: 813
		private static string NoPathTrans;

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00046D2C File Offset: 0x0004512C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00046D44 File Offset: 0x00045144
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00046D5A File Offset: 0x0004515A
		public static void ResetStaticData()
		{
			WorkGiver_Miner.NoPathTrans = "NoPath".Translate();
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00046D6C File Offset: 0x0004516C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine))
			{
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
					Mineable i = des.target.Cell.GetFirstMineable(pawn.Map);
					if (i != null)
					{
						yield return i;
					}
				}
			}
			yield break;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00046D98 File Offset: 0x00045198
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
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
						if (intVec2.InBounds(t.Map))
						{
							if (ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn))
							{
								if (intVec2.Walkable(t.Map) && !intVec2.Standable(t.Map))
								{
									Thing thing = null;
									List<Thing> thingList = intVec2.GetThingList(t.Map);
									for (int k = 0; k < thingList.Count; k++)
									{
										if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
										{
											thing = thingList[k];
											break;
										}
									}
									if (thing != null)
									{
										Job job = HaulAIUtility.HaulAsideJobFor(pawn, thing);
										if (job != null)
										{
											return job;
										}
										JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
										return null;
									}
								}
							}
						}
					}
					JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Mine, t, 1500, true);
				}
			}
			return result;
		}
	}
}
