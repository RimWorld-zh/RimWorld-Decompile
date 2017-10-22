using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class SpectatorCellFinder
	{
		private const float MaxDistanceToSpectateRect = 14.5f;

		private static float[] scorePerSide = new float[4];

		private static List<IntVec3> usedCells = new List<IntVec3>();

		public static bool TryFindSpectatorCellFor(Pawn p, CellRect spectateRect, Map map, out IntVec3 cell, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1, List<IntVec3> extraDisallowedCells = null)
		{
			spectateRect.ClipInsideMap(map);
			bool result;
			Predicate<IntVec3> predicate;
			IntVec3 intVec;
			if (spectateRect.Area == 0 || allowedSides == SpectateRectSide.None)
			{
				cell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				CellRect rectWithMargin = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
				predicate = (Predicate<IntVec3>)delegate(IntVec3 x)
				{
					bool result2;
					CellRect rectWithMargin2;
					if (!x.InBounds(map))
					{
						result2 = false;
					}
					else if (!x.Standable(map))
					{
						result2 = false;
					}
					else if (x.Fogged(map))
					{
						result2 = false;
					}
					else if (rectWithMargin2.Contains(x))
					{
						result2 = false;
					}
					else
					{
						if (x.z > rectWithMargin2.maxZ && ((int)allowedSides & 1) == 1)
						{
							goto IL_00fd;
						}
						if (x.x > rectWithMargin2.maxX && ((int)allowedSides & 2) == 2)
						{
							goto IL_00fd;
						}
						if (x.z < rectWithMargin2.minZ && ((int)allowedSides & 4) == 4)
						{
							goto IL_00fd;
						}
						if (x.x < rectWithMargin2.minX && ((int)allowedSides & 8) == 8)
						{
							goto IL_00fd;
						}
						result2 = false;
					}
					goto IL_0397;
					IL_00fd:
					CellRect spectateRect2;
					IntVec3 intVec3 = spectateRect2.ClosestCellTo(x);
					if ((float)intVec3.DistanceToSquared(x) > 210.25)
					{
						result2 = false;
					}
					else if (!GenSight.LineOfSight(intVec3, x, map, true, null, 0, 0))
					{
						result2 = false;
					}
					else if (x.GetThingList(map).Find((Predicate<Thing>)((Thing y) => y is Pawn && y != p)) != null)
					{
						result2 = false;
					}
					else
					{
						if (p != null)
						{
							if (!p.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
							{
								result2 = false;
								goto IL_0397;
							}
							Building edifice = x.GetEdifice(map);
							if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable && !p.CanReserve((Thing)edifice, 1, -1, null, false))
							{
								result2 = false;
								goto IL_0397;
							}
							if (x.IsForbidden(p))
							{
								result2 = false;
								goto IL_0397;
							}
							if (x.GetDangerFor(p, map) != Danger.None)
							{
								result2 = false;
								goto IL_0397;
							}
						}
						if (extraDisallowedCells != null && extraDisallowedCells.Contains(x))
						{
							result2 = false;
						}
						else
						{
							if (!SpectatorCellFinder.CorrectlyRotatedChairAt(x, map, spectateRect2))
							{
								int num = 0;
								for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
								{
									IntVec3 x2 = x + GenAdj.AdjacentCells[k];
									if (SpectatorCellFinder.CorrectlyRotatedChairAt(x2, map, spectateRect2))
									{
										num++;
									}
								}
								if (num >= 3)
								{
									result2 = false;
									goto IL_0397;
								}
								int num2 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(-1, 0, 0), map, 4, spectateRect2);
								if (num2 >= 0)
								{
									int num3 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(1, 0, 0), map, 4, spectateRect2);
									if (num3 >= 0 && Mathf.Abs(num2 - num3) <= 1)
									{
										result2 = false;
										goto IL_0397;
									}
								}
								int num4 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, 1), map, 4, spectateRect2);
								if (num4 >= 0)
								{
									int num5 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, -1), map, 4, spectateRect2);
									if (num5 >= 0 && Mathf.Abs(num4 - num5) <= 1)
									{
										result2 = false;
										goto IL_0397;
									}
								}
							}
							result2 = true;
						}
					}
					goto IL_0397;
					IL_0397:
					return result2;
				};
				if (p != null && predicate(p.Position) && SpectatorCellFinder.CorrectlyRotatedChairAt(p.Position, map, spectateRect))
				{
					cell = p.Position;
					result = true;
				}
				else
				{
					for (int i = 0; i < 1000; i++)
					{
						intVec = rectWithMargin.CenterCell + GenRadial.RadialPattern[i];
						if (predicate(intVec))
							goto IL_0130;
					}
					cell = IntVec3.Invalid;
					result = false;
				}
			}
			goto IL_01e7;
			IL_0130:
			IntVec3 intVec2;
			if (!SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
			{
				for (int j = 0; j < 90; j++)
				{
					intVec2 = intVec + GenRadial.RadialPattern[j];
					if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec2, map, spectateRect) && predicate(intVec2))
						goto IL_0192;
				}
			}
			cell = intVec;
			result = true;
			goto IL_01e7;
			IL_01e7:
			return result;
			IL_0192:
			cell = intVec2;
			result = true;
			goto IL_01e7;
		}

		private static bool CorrectlyRotatedChairAt(IntVec3 x, Map map, CellRect spectateRect)
		{
			return SpectatorCellFinder.GetCorrectlyRotatedChairAt(x, map, spectateRect) != null;
		}

		private static Building GetCorrectlyRotatedChairAt(IntVec3 x, Map map, CellRect spectateRect)
		{
			Building result;
			if (!x.InBounds(map))
			{
				result = null;
			}
			else
			{
				Building edifice = x.GetEdifice(map);
				if (edifice == null || edifice.def.category != ThingCategory.Building || !edifice.def.building.isSittable)
				{
					result = null;
				}
				else
				{
					float num = GenGeo.AngleDifferenceBetween(edifice.Rotation.AsAngle, (spectateRect.ClosestCellTo(x) - edifice.Position).AngleFlat);
					result = ((!(num > 75.0)) ? edifice : null);
				}
			}
			return result;
		}

		private static int DistanceToClosestChair(IntVec3 from, IntVec3 step, Map map, int maxDist, CellRect spectateRect)
		{
			int num = 0;
			IntVec3 intVec = from;
			int result;
			while (true)
			{
				intVec += step;
				num++;
				if (!intVec.InBounds(map))
				{
					result = -1;
				}
				else if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
				{
					result = num;
				}
				else if (!intVec.Walkable(map))
				{
					result = -1;
				}
				else
				{
					if (num < maxDist)
						continue;
					result = -1;
				}
				break;
			}
			return result;
		}

		public static void DebugFlashPotentialSpectatorCells(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1)
		{
			List<IntVec3> list = new List<IntVec3>();
			int num = 50;
			int num2 = 0;
			IntVec3 intVec = default(IntVec3);
			while (num2 < num && SpectatorCellFinder.TryFindSpectatorCellFor((Pawn)null, spectateRect, map, out intVec, allowedSides, margin, list))
			{
				list.Add(intVec);
				float a = Mathf.Lerp(1f, 0.08f, (float)num2 / (float)num);
				Material mat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0.8f, 0f, a), false);
				map.debugDrawer.FlashCell(intVec, mat, (num2 + 1).ToString(), 50);
				num2++;
			}
			SpectateRectSide spectateRectSide = SpectatorCellFinder.FindSingleBestSide(spectateRect, map, allowedSides, margin);
			IntVec3 centerCell = spectateRect.CenterCell;
			switch (spectateRectSide)
			{
			case SpectateRectSide.Up:
			{
				centerCell.z += spectateRect.Height / 2 + 10;
				break;
			}
			case SpectateRectSide.Right:
			{
				centerCell.x += spectateRect.Width / 2 + 10;
				break;
			}
			case SpectateRectSide.Down:
			{
				centerCell.z -= spectateRect.Height / 2 + 10;
				break;
			}
			case SpectateRectSide.Left:
			{
				centerCell.x -= spectateRect.Width / 2 + 10;
				break;
			}
			}
			map.debugDrawer.FlashLine(spectateRect.CenterCell, centerCell, 50);
		}

		public static SpectateRectSide FindSingleBestSide(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1)
		{
			for (int i = 0; i < SpectatorCellFinder.scorePerSide.Length; i++)
			{
				SpectatorCellFinder.scorePerSide[i] = 0f;
			}
			SpectatorCellFinder.usedCells.Clear();
			int num = 30;
			CellRect cellRect = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
			int num2 = 0;
			IntVec3 intVec = default(IntVec3);
			while (num2 < num && SpectatorCellFinder.TryFindSpectatorCellFor((Pawn)null, spectateRect, map, out intVec, allowedSides, margin, SpectatorCellFinder.usedCells))
			{
				SpectatorCellFinder.usedCells.Add(intVec);
				float num3;
				float num4 = num3 = Mathf.Lerp(1f, 0.35f, (float)num2 / (float)num);
				Building correctlyRotatedChairAt = SpectatorCellFinder.GetCorrectlyRotatedChairAt(intVec, map, spectateRect);
				if (intVec.z > cellRect.maxZ && ((int)allowedSides & 1) == 1)
				{
					SpectatorCellFinder.scorePerSide[0] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.South)
					{
						SpectatorCellFinder.scorePerSide[0] += (float)(1.2000000476837158 * num4);
					}
				}
				if (intVec.x > cellRect.maxX && ((int)allowedSides & 2) == 2)
				{
					SpectatorCellFinder.scorePerSide[1] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.West)
					{
						SpectatorCellFinder.scorePerSide[1] += (float)(1.2000000476837158 * num4);
					}
				}
				if (intVec.z < cellRect.minZ && ((int)allowedSides & 4) == 4)
				{
					SpectatorCellFinder.scorePerSide[2] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.North)
					{
						SpectatorCellFinder.scorePerSide[2] += (float)(1.2000000476837158 * num4);
					}
				}
				if (intVec.x < cellRect.minX && ((int)allowedSides & 8) == 8)
				{
					SpectatorCellFinder.scorePerSide[3] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.East)
					{
						SpectatorCellFinder.scorePerSide[3] += (float)(1.2000000476837158 * num4);
					}
				}
				num2++;
			}
			float num5 = 0f;
			int num6 = -1;
			for (int j = 0; j < SpectatorCellFinder.scorePerSide.Length; j++)
			{
				if (SpectatorCellFinder.scorePerSide[j] != 0.0 && (num6 < 0 || SpectatorCellFinder.scorePerSide[j] > num5))
				{
					num6 = j;
					num5 = SpectatorCellFinder.scorePerSide[j];
				}
			}
			SpectatorCellFinder.usedCells.Clear();
			SpectateRectSide result;
			switch (num6)
			{
			case 0:
			{
				result = SpectateRectSide.Up;
				break;
			}
			case 1:
			{
				result = SpectateRectSide.Right;
				break;
			}
			case 2:
			{
				result = SpectateRectSide.Down;
				break;
			}
			case 3:
			{
				result = SpectateRectSide.Left;
				break;
			}
			default:
			{
				result = SpectateRectSide.None;
				break;
			}
			}
			return result;
		}
	}
}
