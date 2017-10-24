using RimWorld;
using System.Collections.Generic;

namespace Verse.AI
{
	public static class PawnPathUtility
	{
		public static Thing FirstBlockingBuilding(this PawnPath path, out IntVec3 cellBefore, Pawn pawn = null)
		{
			Thing result;
			List<IntVec3> nodesReversed;
			Building building;
			IntVec3 intVec;
			int num;
			Building edifice;
			if (!path.Found)
			{
				cellBefore = IntVec3.Invalid;
				result = null;
			}
			else
			{
				nodesReversed = path.NodesReversed;
				if (nodesReversed.Count == 1)
				{
					cellBefore = nodesReversed[0];
					result = null;
				}
				else
				{
					building = null;
					intVec = IntVec3.Invalid;
					for (num = nodesReversed.Count - 2; num >= 0; num--)
					{
						edifice = nodesReversed[num].GetEdifice(pawn.Map);
						if (edifice != null)
						{
							Building_Door building_Door = edifice as Building_Door;
							if ((building_Door == null || building_Door.FreePassage || (pawn != null && building_Door.PawnCanOpen(pawn))) && edifice.def.passability != Traversability.Impassable)
							{
								goto IL_00fa;
							}
							goto IL_00ca;
						}
						goto IL_00fa;
						IL_00fa:
						if (edifice != null && edifice.def.passability == Traversability.PassThroughOnly && edifice.def.Fillage == FillCategory.Full)
						{
							if (building == null)
							{
								building = edifice;
								intVec = nodesReversed[num + 1];
							}
						}
						else if (edifice == null || edifice.def.passability != Traversability.PassThroughOnly)
						{
							building = null;
						}
					}
					cellBefore = nodesReversed[0];
					result = null;
				}
			}
			goto IL_0189;
			IL_0189:
			return result;
			IL_00ca:
			if (building != null)
			{
				cellBefore = intVec;
				result = building;
			}
			else
			{
				cellBefore = nodesReversed[num + 1];
				result = edifice;
			}
			goto IL_0189;
		}

		public static IntVec3 FinalWalkableNonDoorCell(this PawnPath path, Map map)
		{
			IntVec3 result;
			List<IntVec3> nodesReversed;
			int i;
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
			}
			else
			{
				nodesReversed = path.NodesReversed;
				for (i = 0; i < nodesReversed.Count; i++)
				{
					Building edifice = nodesReversed[i].GetEdifice(map);
					if (edifice != null && edifice.def.passability == Traversability.Impassable)
					{
						continue;
					}
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door != null && !building_Door.FreePassage)
					{
						continue;
					}
					goto IL_0074;
				}
				result = nodesReversed[0];
			}
			goto IL_00a0;
			IL_0074:
			result = nodesReversed[i];
			goto IL_00a0;
			IL_00a0:
			return result;
		}

		public static IntVec3 LastCellBeforeBlockerOrFinalCell(this PawnPath path, Map map)
		{
			IntVec3 result;
			List<IntVec3> nodesReversed;
			int num;
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
			}
			else
			{
				nodesReversed = path.NodesReversed;
				for (num = nodesReversed.Count - 2; num >= 1; num--)
				{
					Building edifice = nodesReversed[num].GetEdifice(map);
					if (edifice != null)
					{
						if (edifice.def.passability == Traversability.Impassable)
							goto IL_0060;
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door != null && !building_Door.FreePassage)
							goto IL_008a;
					}
				}
				result = nodesReversed[0];
			}
			goto IL_00b3;
			IL_008a:
			result = nodesReversed[num + 1];
			goto IL_00b3;
			IL_0060:
			result = nodesReversed[num + 1];
			goto IL_00b3;
			IL_00b3:
			return result;
		}

		public static bool TryFindLastCellBeforeBlockingDoor(this PawnPath path, Pawn pawn, out IntVec3 result)
		{
			bool result2;
			List<IntVec3> nodesReversed;
			int num;
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
				result2 = false;
			}
			else
			{
				nodesReversed = path.NodesReversed;
				for (num = nodesReversed.Count - 2; num >= 1; num--)
				{
					Building_Door building_Door = nodesReversed[num].GetEdifice(pawn.Map) as Building_Door;
					if (building_Door != null && !building_Door.CanPhysicallyPass(pawn))
						goto IL_006d;
				}
				result = nodesReversed[0];
				result2 = false;
			}
			goto IL_00a5;
			IL_00a5:
			return result2;
			IL_006d:
			result = nodesReversed[num + 1];
			result2 = true;
			goto IL_00a5;
		}

		public static bool TryFindCellAtIndex(PawnPath path, int index, out IntVec3 result)
		{
			bool result2;
			if (path.NodesReversed.Count <= index || index < 0)
			{
				result = IntVec3.Invalid;
				result2 = false;
			}
			else
			{
				result = path.NodesReversed[path.NodesReversed.Count - 1 - index];
				result2 = true;
			}
			return result2;
		}
	}
}
