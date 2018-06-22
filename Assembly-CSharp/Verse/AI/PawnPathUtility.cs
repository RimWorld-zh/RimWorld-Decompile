using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A94 RID: 2708
	public static class PawnPathUtility
	{
		// Token: 0x06003C3A RID: 15418 RVA: 0x001FD140 File Offset: 0x001FB540
		public static Thing FirstBlockingBuilding(this PawnPath path, out IntVec3 cellBefore, Pawn pawn = null)
		{
			Thing result;
			if (!path.Found)
			{
				cellBefore = IntVec3.Invalid;
				result = null;
			}
			else
			{
				List<IntVec3> nodesReversed = path.NodesReversed;
				if (nodesReversed.Count == 1)
				{
					cellBefore = nodesReversed[0];
					result = null;
				}
				else
				{
					Building building = null;
					IntVec3 intVec = IntVec3.Invalid;
					for (int i = nodesReversed.Count - 2; i >= 0; i--)
					{
						Building edifice = nodesReversed[i].GetEdifice(pawn.Map);
						if (edifice != null)
						{
							Building_Door building_Door = edifice as Building_Door;
							if ((building_Door != null && !building_Door.FreePassage && (pawn == null || !building_Door.PawnCanOpen(pawn))) || edifice.def.passability == Traversability.Impassable)
							{
								if (building != null)
								{
									cellBefore = intVec;
									return building;
								}
								cellBefore = nodesReversed[i + 1];
								return edifice;
							}
						}
						if (edifice != null && edifice.def.passability == Traversability.PassThroughOnly && edifice.def.Fillage == FillCategory.Full)
						{
							if (building == null)
							{
								building = edifice;
								intVec = nodesReversed[i + 1];
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
			return result;
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x001FD2D8 File Offset: 0x001FB6D8
		public static IntVec3 FinalWalkableNonDoorCell(this PawnPath path, Map map)
		{
			IntVec3 result;
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
			}
			else
			{
				List<IntVec3> nodesReversed = path.NodesReversed;
				for (int i = 0; i < nodesReversed.Count; i++)
				{
					Building edifice = nodesReversed[i].GetEdifice(map);
					if (edifice == null || edifice.def.passability != Traversability.Impassable)
					{
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door == null || building_Door.FreePassage)
						{
							return nodesReversed[i];
						}
					}
				}
				result = nodesReversed[0];
			}
			return result;
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x001FD388 File Offset: 0x001FB788
		public static IntVec3 LastCellBeforeBlockerOrFinalCell(this PawnPath path, Map map)
		{
			IntVec3 result;
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
			}
			else
			{
				List<IntVec3> nodesReversed = path.NodesReversed;
				for (int i = nodesReversed.Count - 2; i >= 1; i--)
				{
					Building edifice = nodesReversed[i].GetEdifice(map);
					if (edifice != null)
					{
						if (edifice.def.passability == Traversability.Impassable)
						{
							return nodesReversed[i + 1];
						}
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door != null && !building_Door.FreePassage)
						{
							return nodesReversed[i + 1];
						}
					}
				}
				result = nodesReversed[0];
			}
			return result;
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x001FD44C File Offset: 0x001FB84C
		public static bool TryFindLastCellBeforeBlockingDoor(this PawnPath path, Pawn pawn, out IntVec3 result)
		{
			bool result2;
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
				result2 = false;
			}
			else
			{
				List<IntVec3> nodesReversed = path.NodesReversed;
				for (int i = nodesReversed.Count - 2; i >= 1; i--)
				{
					Building_Door building_Door = nodesReversed[i].GetEdifice(pawn.Map) as Building_Door;
					if (building_Door != null)
					{
						if (!building_Door.CanPhysicallyPass(pawn))
						{
							result = nodesReversed[i + 1];
							return true;
						}
					}
				}
				result = nodesReversed[0];
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x001FD500 File Offset: 0x001FB900
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
