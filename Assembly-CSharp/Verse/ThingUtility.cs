using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public static class ThingUtility
	{
		public static bool DestroyedOrNull(this Thing t)
		{
			return t == null || t.Destroyed;
		}

		public static void DestroyOrPassToWorld(this Thing t, DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			else
			{
				t.Destroy(mode);
			}
		}

		public static int TryAbsorbStackNumToTake(Thing thing, Thing other, bool respectStackLimit)
		{
			return (!respectStackLimit) ? other.stackCount : Mathf.Min(other.stackCount, thing.def.stackLimit - thing.stackCount);
		}

		public static int RoundedResourceStackCount(int stackCount)
		{
			return (stackCount <= 200) ? ((stackCount <= 100) ? stackCount : GenMath.RoundTo(stackCount, 5)) : GenMath.RoundTo(stackCount, 10);
		}

		public static IntVec3 InteractionCellWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			IntVec3 result;
			IntVec3 intVec;
			IntVec3 intVec2;
			IntVec3 intVec3;
			List<IntVec3> list;
			int l;
			int m;
			int n;
			if (def.hasInteractionCell)
			{
				IntVec3 b = def.interactionCellOffset.RotatedBy(rot);
				result = center + b;
			}
			else
			{
				IntVec2 size = def.Size;
				if (size.x == 1)
				{
					IntVec2 size2 = def.Size;
					if (size2.z == 1)
					{
						for (int i = 0; i < 8; i++)
						{
							intVec = center + GenAdj.AdjacentCells[i];
							if (intVec.Standable(map) && intVec.GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(intVec, center, map, PathEndMode.Touch, null))
								goto IL_00a1;
						}
						for (int j = 0; j < 8; j++)
						{
							intVec2 = center + GenAdj.AdjacentCells[j];
							if (intVec2.Standable(map) && ReachabilityImmediate.CanReachImmediate(intVec2, center, map, PathEndMode.Touch, null))
								goto IL_00fc;
						}
						for (int k = 0; k < 8; k++)
						{
							intVec3 = center + GenAdj.AdjacentCells[k];
							if (intVec3.Walkable(map) && ReachabilityImmediate.CanReachImmediate(intVec3, center, map, PathEndMode.Touch, null))
								goto IL_0157;
						}
						result = center;
						goto IL_02ba;
					}
				}
				list = GenAdjFast.AdjacentCells8Way(center, rot, def.size);
				CellRect rect = GenAdj.OccupiedRect(center, rot, def.size);
				for (l = 0; l < list.Count; l++)
				{
					if (list[l].Standable(map) && list[l].GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(list[l], rect, map, PathEndMode.Touch, null))
						goto IL_01dd;
				}
				for (m = 0; m < list.Count; m++)
				{
					if (list[m].Standable(map) && ReachabilityImmediate.CanReachImmediate(list[m], rect, map, PathEndMode.Touch, null))
						goto IL_0236;
				}
				for (n = 0; n < list.Count; n++)
				{
					if (list[n].Walkable(map) && ReachabilityImmediate.CanReachImmediate(list[n], rect, map, PathEndMode.Touch, null))
						goto IL_028f;
				}
				result = center;
			}
			goto IL_02ba;
			IL_00fc:
			result = intVec2;
			goto IL_02ba;
			IL_00a1:
			result = intVec;
			goto IL_02ba;
			IL_01dd:
			result = list[l];
			goto IL_02ba;
			IL_028f:
			result = list[n];
			goto IL_02ba;
			IL_0157:
			result = intVec3;
			goto IL_02ba;
			IL_02ba:
			return result;
			IL_0236:
			result = list[m];
			goto IL_02ba;
		}

		public static DamageDef PrimaryMeleeWeaponDamageType(ThingDef thing)
		{
			List<Tool> tools = thing.tools;
			Tool tool2 = tools.MaxBy((Func<Tool, float>)((Tool tool) => tool.power));
			List<ManeuverDef> allDefsListForReading = DefDatabase<ManeuverDef>.AllDefsListForReading;
			int num = 0;
			DamageDef result;
			while (true)
			{
				if (num < allDefsListForReading.Count)
				{
					ManeuverDef maneuverDef = allDefsListForReading[num];
					if (tool2.capacities.Contains(maneuverDef.requiredCapacity))
					{
						result = maneuverDef.verb.meleeDamageDef;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
