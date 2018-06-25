using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
			if (spectateRect.Area == 0 || allowedSides == SpectateRectSide.None)
			{
				cell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				CellRect rectWithMargin = spectateRect.ExpandedBy(margin).ClipInsideMap(map);
				Predicate<IntVec3> predicate = delegate(IntVec3 x)
				{
					bool result2;
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
					else if (rectWithMargin.Contains(x))
					{
						result2 = false;
					}
					else if ((x.z <= rectWithMargin.maxZ || (allowedSides & SpectateRectSide.Up) != SpectateRectSide.Up) && (x.x <= rectWithMargin.maxX || (allowedSides & SpectateRectSide.Right) != SpectateRectSide.Right) && (x.z >= rectWithMargin.minZ || (allowedSides & SpectateRectSide.Down) != SpectateRectSide.Down) && (x.x >= rectWithMargin.minX || (allowedSides & SpectateRectSide.Left) != SpectateRectSide.Left))
					{
						result2 = false;
					}
					else
					{
						IntVec3 intVec3 = spectateRect.ClosestCellTo(x);
						if ((float)intVec3.DistanceToSquared(x) > 210.25f)
						{
							result2 = false;
						}
						else if (!GenSight.LineOfSight(intVec3, x, map, true, null, 0, 0))
						{
							result2 = false;
						}
						else if (x.GetThingList(map).Find((Thing y) => y is Pawn && y != p) != null)
						{
							result2 = false;
						}
						else
						{
							if (p != null)
							{
								if (!p.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
								{
									return false;
								}
								Building edifice = x.GetEdifice(map);
								if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable && !p.CanReserve(edifice, 1, -1, null, false))
								{
									return false;
								}
								if (x.IsForbidden(p))
								{
									return false;
								}
								if (x.GetDangerFor(p, map) != Danger.None)
								{
									return false;
								}
							}
							if (extraDisallowedCells != null && extraDisallowedCells.Contains(x))
							{
								result2 = false;
							}
							else
							{
								if (!SpectatorCellFinder.CorrectlyRotatedChairAt(x, map, spectateRect))
								{
									int num = 0;
									for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
									{
										IntVec3 x2 = x + GenAdj.AdjacentCells[k];
										if (SpectatorCellFinder.CorrectlyRotatedChairAt(x2, map, spectateRect))
										{
											num++;
										}
									}
									if (num >= 3)
									{
										return false;
									}
									int num2 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(-1, 0, 0), map, 4, spectateRect);
									if (num2 >= 0)
									{
										int num3 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(1, 0, 0), map, 4, spectateRect);
										if (num3 >= 0 && Mathf.Abs(num2 - num3) <= 1)
										{
											return false;
										}
									}
									int num4 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, 1), map, 4, spectateRect);
									if (num4 >= 0)
									{
										int num5 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, -1), map, 4, spectateRect);
										if (num5 >= 0 && Mathf.Abs(num4 - num5) <= 1)
										{
											return false;
										}
									}
								}
								result2 = true;
							}
						}
					}
					return result2;
				};
				if (p != null)
				{
					if (predicate(p.Position) && SpectatorCellFinder.CorrectlyRotatedChairAt(p.Position, map, spectateRect))
					{
						cell = p.Position;
						return true;
					}
				}
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 intVec = rectWithMargin.CenterCell + GenRadial.RadialPattern[i];
					if (predicate(intVec))
					{
						if (!SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
						{
							for (int j = 0; j < 90; j++)
							{
								IntVec3 intVec2 = intVec + GenRadial.RadialPattern[j];
								if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec2, map, spectateRect) && predicate(intVec2))
								{
									cell = intVec2;
									return true;
								}
							}
						}
						cell = intVec;
						return true;
					}
				}
				cell = IntVec3.Invalid;
				result = false;
			}
			return result;
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
					if (num > 75f)
					{
						result = null;
					}
					else
					{
						result = edifice;
					}
				}
			}
			return result;
		}

		private static int DistanceToClosestChair(IntVec3 from, IntVec3 step, Map map, int maxDist, CellRect spectateRect)
		{
			int num = 0;
			IntVec3 intVec = from;
			for (;;)
			{
				intVec += step;
				num++;
				if (!intVec.InBounds(map))
				{
					break;
				}
				if (SpectatorCellFinder.CorrectlyRotatedChairAt(intVec, map, spectateRect))
				{
					goto Block_2;
				}
				if (!intVec.Walkable(map))
				{
					goto Block_3;
				}
				if (num >= maxDist)
				{
					goto Block_4;
				}
			}
			return -1;
			Block_2:
			return num;
			Block_3:
			return -1;
			Block_4:
			return -1;
		}

		public static void DebugFlashPotentialSpectatorCells(CellRect spectateRect, Map map, SpectateRectSide allowedSides = SpectateRectSide.All, int margin = 1)
		{
			List<IntVec3> list = new List<IntVec3>();
			int num = 50;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!SpectatorCellFinder.TryFindSpectatorCellFor(null, spectateRect, map, out intVec, allowedSides, margin, list))
				{
					break;
				}
				list.Add(intVec);
				float a = Mathf.Lerp(1f, 0.08f, (float)i / (float)num);
				Material mat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0.8f, 0f, a), false);
				map.debugDrawer.FlashCell(intVec, mat, (i + 1).ToString(), 50);
			}
			SpectateRectSide spectateRectSide = SpectatorCellFinder.FindSingleBestSide(spectateRect, map, allowedSides, margin);
			IntVec3 centerCell = spectateRect.CenterCell;
			switch (spectateRectSide)
			{
			case SpectateRectSide.Up:
				centerCell.z += spectateRect.Height / 2 + 10;
				break;
			case SpectateRectSide.Right:
				centerCell.x += spectateRect.Width / 2 + 10;
				break;
			case SpectateRectSide.Down:
				centerCell.z -= spectateRect.Height / 2 + 10;
				break;
			case SpectateRectSide.Left:
				centerCell.x -= spectateRect.Width / 2 + 10;
				break;
			}
			map.debugDrawer.FlashLine(spectateRect.CenterCell, centerCell, 50, SimpleColor.White);
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
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec;
				if (!SpectatorCellFinder.TryFindSpectatorCellFor(null, spectateRect, map, out intVec, allowedSides, margin, SpectatorCellFinder.usedCells))
				{
					break;
				}
				SpectatorCellFinder.usedCells.Add(intVec);
				float num2 = Mathf.Lerp(1f, 0.35f, (float)j / (float)num);
				float num3 = num2;
				Building correctlyRotatedChairAt = SpectatorCellFinder.GetCorrectlyRotatedChairAt(intVec, map, spectateRect);
				if (intVec.z > cellRect.maxZ && (allowedSides & SpectateRectSide.Up) == SpectateRectSide.Up)
				{
					SpectatorCellFinder.scorePerSide[0] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.South)
					{
						SpectatorCellFinder.scorePerSide[0] += 1.2f * num2;
					}
				}
				if (intVec.x > cellRect.maxX && (allowedSides & SpectateRectSide.Right) == SpectateRectSide.Right)
				{
					SpectatorCellFinder.scorePerSide[1] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.West)
					{
						SpectatorCellFinder.scorePerSide[1] += 1.2f * num2;
					}
				}
				if (intVec.z < cellRect.minZ && (allowedSides & SpectateRectSide.Down) == SpectateRectSide.Down)
				{
					SpectatorCellFinder.scorePerSide[2] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.North)
					{
						SpectatorCellFinder.scorePerSide[2] += 1.2f * num2;
					}
				}
				if (intVec.x < cellRect.minX && (allowedSides & SpectateRectSide.Left) == SpectateRectSide.Left)
				{
					SpectatorCellFinder.scorePerSide[3] += num3;
					if (correctlyRotatedChairAt != null && correctlyRotatedChairAt.Rotation == Rot4.East)
					{
						SpectatorCellFinder.scorePerSide[3] += 1.2f * num2;
					}
				}
			}
			float num4 = 0f;
			int num5 = -1;
			for (int k = 0; k < SpectatorCellFinder.scorePerSide.Length; k++)
			{
				if (SpectatorCellFinder.scorePerSide[k] != 0f)
				{
					if (num5 < 0 || SpectatorCellFinder.scorePerSide[k] > num4)
					{
						num5 = k;
						num4 = SpectatorCellFinder.scorePerSide[k];
					}
				}
			}
			SpectatorCellFinder.usedCells.Clear();
			SpectateRectSide result;
			switch (num5)
			{
			case 0:
				result = SpectateRectSide.Up;
				break;
			case 1:
				result = SpectateRectSide.Right;
				break;
			case 2:
				result = SpectateRectSide.Down;
				break;
			case 3:
				result = SpectateRectSide.Left;
				break;
			default:
				result = SpectateRectSide.None;
				break;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SpectatorCellFinder()
		{
		}

		[CompilerGenerated]
		private sealed class <TryFindSpectatorCellFor>c__AnonStorey0
		{
			internal Map map;

			internal CellRect rectWithMargin;

			internal SpectateRectSide allowedSides;

			internal CellRect spectateRect;

			internal Pawn p;

			internal List<IntVec3> extraDisallowedCells;

			public <TryFindSpectatorCellFor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				bool result;
				if (!x.InBounds(this.map))
				{
					result = false;
				}
				else if (!x.Standable(this.map))
				{
					result = false;
				}
				else if (x.Fogged(this.map))
				{
					result = false;
				}
				else if (this.rectWithMargin.Contains(x))
				{
					result = false;
				}
				else if ((x.z <= this.rectWithMargin.maxZ || (this.allowedSides & SpectateRectSide.Up) != SpectateRectSide.Up) && (x.x <= this.rectWithMargin.maxX || (this.allowedSides & SpectateRectSide.Right) != SpectateRectSide.Right) && (x.z >= this.rectWithMargin.minZ || (this.allowedSides & SpectateRectSide.Down) != SpectateRectSide.Down) && (x.x >= this.rectWithMargin.minX || (this.allowedSides & SpectateRectSide.Left) != SpectateRectSide.Left))
				{
					result = false;
				}
				else
				{
					IntVec3 intVec = this.spectateRect.ClosestCellTo(x);
					if ((float)intVec.DistanceToSquared(x) > 210.25f)
					{
						result = false;
					}
					else if (!GenSight.LineOfSight(intVec, x, this.map, true, null, 0, 0))
					{
						result = false;
					}
					else if (x.GetThingList(this.map).Find((Thing y) => y is Pawn && y != this.p) != null)
					{
						result = false;
					}
					else
					{
						if (this.p != null)
						{
							if (!this.p.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
							{
								return false;
							}
							Building edifice = x.GetEdifice(this.map);
							if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable && !this.p.CanReserve(edifice, 1, -1, null, false))
							{
								return false;
							}
							if (x.IsForbidden(this.p))
							{
								return false;
							}
							if (x.GetDangerFor(this.p, this.map) != Danger.None)
							{
								return false;
							}
						}
						if (this.extraDisallowedCells != null && this.extraDisallowedCells.Contains(x))
						{
							result = false;
						}
						else
						{
							if (!SpectatorCellFinder.CorrectlyRotatedChairAt(x, this.map, this.spectateRect))
							{
								int num = 0;
								for (int i = 0; i < GenAdj.AdjacentCells.Length; i++)
								{
									IntVec3 x2 = x + GenAdj.AdjacentCells[i];
									if (SpectatorCellFinder.CorrectlyRotatedChairAt(x2, this.map, this.spectateRect))
									{
										num++;
									}
								}
								if (num >= 3)
								{
									return false;
								}
								int num2 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(-1, 0, 0), this.map, 4, this.spectateRect);
								if (num2 >= 0)
								{
									int num3 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(1, 0, 0), this.map, 4, this.spectateRect);
									if (num3 >= 0 && Mathf.Abs(num2 - num3) <= 1)
									{
										return false;
									}
								}
								int num4 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, 1), this.map, 4, this.spectateRect);
								if (num4 >= 0)
								{
									int num5 = SpectatorCellFinder.DistanceToClosestChair(x, new IntVec3(0, 0, -1), this.map, 4, this.spectateRect);
									if (num5 >= 0 && Mathf.Abs(num4 - num5) <= 1)
									{
										return false;
									}
								}
							}
							result = true;
						}
					}
				}
				return result;
			}

			internal bool <>m__1(Thing y)
			{
				return y is Pawn && y != this.p;
			}
		}
	}
}
