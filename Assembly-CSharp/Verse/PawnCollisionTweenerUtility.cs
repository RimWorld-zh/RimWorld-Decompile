using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class PawnCollisionTweenerUtility
	{
		private const float Radius = 0.32f;

		public static Vector3 PawnCollisionPosOffsetFor(Pawn pawn)
		{
			Vector3 result;
			if (pawn.GetPosture() != 0)
			{
				result = Vector3.zero;
			}
			else
			{
				bool flag = pawn.Spawned && pawn.pather.MovingNow;
				if (!flag || pawn.pather.nextCell == pawn.pather.Destination.Cell)
				{
					if (!flag && pawn.Drawer.leaner.ShouldLean())
					{
						result = Vector3.zero;
					}
					else
					{
						IntVec3 at = (!flag) ? pawn.Position : pawn.pather.nextCell;
						int polygonVertices = default(int);
						int vertexIndex = default(int);
						bool flag2 = default(bool);
						PawnCollisionTweenerUtility.GetPawnsStandingAtOrAboutToStandAt(at, pawn.Map, out polygonVertices, out vertexIndex, out flag2, pawn);
						result = (flag2 ? (GenGeo.RegularPolygonVertexPositionVec3(polygonVertices, vertexIndex) * 0.32f) : Vector3.zero);
					}
				}
				else
				{
					IntVec3 nextCell = pawn.pather.nextCell;
					if (PawnCollisionTweenerUtility.CanGoDirectlyToNextCell(pawn))
					{
						result = Vector3.zero;
					}
					else
					{
						int num = pawn.thingIDNumber % 2;
						int x = nextCell.x;
						IntVec3 position = pawn.Position;
						result = ((x == position.x) ? ((num != 0) ? new Vector3(-0.32f, 0f, 0f) : new Vector3(0.32f, 0f, 0f)) : ((num != 0) ? new Vector3(0f, 0f, -0.32f) : new Vector3(0f, 0f, 0.32f)));
					}
				}
			}
			return result;
		}

		private static void GetPawnsStandingAtOrAboutToStandAt(IntVec3 at, Map map, out int pawnsCount, out int pawnsWithLowerIdCount, out bool forPawnFound, Pawn forPawn)
		{
			pawnsCount = 0;
			pawnsWithLowerIdCount = 0;
			forPawnFound = false;
			CellRect.CellRectIterator iterator = CellRect.SingleCell(at).ExpandedBy(1).GetIterator();
			while (!iterator.Done())
			{
				IntVec3 current = iterator.Current;
				if (current.InBounds(map))
				{
					List<Thing> thingList = current.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn.GetPosture() == PawnPosture.Standing)
						{
							if (current != at)
							{
								if (pawn.pather.MovingNow && !(pawn.pather.nextCell != pawn.pather.Destination.Cell) && !(pawn.pather.Destination.Cell != at))
									goto IL_010b;
							}
							else if (!pawn.pather.MovingNow)
								goto IL_010b;
						}
						continue;
						IL_010b:
						if (pawn == forPawn)
						{
							forPawnFound = true;
						}
						pawnsCount++;
						if (pawn.thingIDNumber < forPawn.thingIDNumber)
						{
							pawnsWithLowerIdCount++;
						}
					}
				}
				iterator.MoveNext();
			}
		}

		private static bool CanGoDirectlyToNextCell(Pawn pawn)
		{
			IntVec3 nextCell = pawn.pather.nextCell;
			CellRect.CellRectIterator iterator = CellRect.FromLimits(nextCell, pawn.Position).ExpandedBy(1).GetIterator();
			bool result;
			while (true)
			{
				if (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					if (current.InBounds(pawn.Map))
					{
						List<Thing> thingList = current.GetThingList(pawn.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Pawn pawn2 = thingList[i] as Pawn;
							if (pawn2 != null && pawn2 != pawn && pawn2.GetPosture() == PawnPosture.Standing)
							{
								if (!pawn2.pather.MovingNow)
								{
									if (!(pawn2.Position == pawn.Position) && !(pawn2.Position == nextCell))
									{
										continue;
									}
									goto IL_0171;
								}
								if ((!(pawn2.Position == nextCell) || !PawnCollisionTweenerUtility.WillBeFasterOnNextCell(pawn, pawn2)) && !(pawn2.pather.nextCell == nextCell) && !(pawn2.Position == pawn.Position) && (!(pawn2.pather.nextCell == pawn.Position) || !PawnCollisionTweenerUtility.WillBeFasterOnNextCell(pawn2, pawn)))
								{
									continue;
								}
								if (pawn2.thingIDNumber < pawn.thingIDNumber)
									goto IL_0139;
							}
						}
					}
					iterator.MoveNext();
					continue;
				}
				result = true;
				break;
				IL_0139:
				result = false;
				break;
				IL_0171:
				result = false;
				break;
			}
			return result;
		}

		private static bool WillBeFasterOnNextCell(Pawn p1, Pawn p2)
		{
			return (p1.pather.nextCellCostLeft != p2.pather.nextCellCostLeft) ? (p1.pather.nextCellCostLeft < p2.pather.nextCellCostLeft) : (p1.thingIDNumber < p2.thingIDNumber);
		}
	}
}
