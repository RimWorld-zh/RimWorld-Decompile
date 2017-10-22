using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class WalkPathFinder
	{
		private const int NumPathNodes = 8;

		private const float StepDistMin = 2f;

		private const float StepDistMax = 14f;

		private static readonly int StartRadialIndex = GenRadial.NumCellsInRadius(14f);

		private static readonly int EndRadialIndex = GenRadial.NumCellsInRadius(2f);

		private static readonly int RadialIndexStride = 3;

		public static bool TryFindWalkPath(Pawn pawn, IntVec3 root, out List<IntVec3> result)
		{
			List<IntVec3> list = new List<IntVec3>();
			list.Add(root);
			IntVec3 intVec = root;
			int num = 0;
			bool result2;
			while (true)
			{
				if (num < 8)
				{
					IntVec3 intVec2 = IntVec3.Invalid;
					float num2 = -1f;
					for (int num3 = WalkPathFinder.StartRadialIndex; num3 > WalkPathFinder.EndRadialIndex; num3 -= WalkPathFinder.RadialIndexStride)
					{
						IntVec3 intVec3 = intVec + GenRadial.RadialPattern[num3];
						if (intVec3.InBounds(pawn.Map) && intVec3.Standable(pawn.Map) && !intVec3.IsForbidden(pawn) && GenSight.LineOfSight(intVec, intVec3, pawn.Map, false, null, 0, 0) && !intVec3.Roofed(pawn.Map) && !PawnUtility.KnownDangerAt(intVec3, pawn))
						{
							float num4 = 10000f;
							for (int i = 0; i < list.Count; i++)
							{
								num4 += (float)(list[i] - intVec3).LengthManhattan;
							}
							float num5 = (float)(intVec3 - root).LengthManhattan;
							if (num5 > 40.0)
							{
								num4 *= Mathf.InverseLerp(70f, 40f, num5);
							}
							if (list.Count >= 2)
							{
								float angleFlat = (list[list.Count - 1] - list[list.Count - 2]).AngleFlat;
								float angleFlat2 = (intVec3 - intVec).AngleFlat;
								float num6;
								if (angleFlat2 > angleFlat)
								{
									num6 = angleFlat2 - angleFlat;
								}
								else
								{
									angleFlat = (float)(angleFlat - 360.0);
									num6 = angleFlat2 - angleFlat;
								}
								if (num6 > 110.0)
								{
									num4 = (float)(num4 * 0.0099999997764825821);
								}
							}
							if (list.Count >= 4 && (intVec - root).LengthManhattan < (intVec3 - root).LengthManhattan)
							{
								num4 = (float)(num4 * 9.9999997473787516E-06);
							}
							if (num4 > num2)
							{
								intVec2 = intVec3;
								num2 = num4;
							}
						}
					}
					if (num2 < 0.0)
					{
						result = null;
						result2 = false;
						break;
					}
					list.Add(intVec2);
					intVec = intVec2;
					num++;
					continue;
				}
				list.Add(root);
				result = list;
				result2 = true;
				break;
			}
			return result2;
		}

		public static void DebugFlashWalkPath(IntVec3 root, int numEntries = 8)
		{
			Map visibleMap = Find.VisibleMap;
			List<IntVec3> list = default(List<IntVec3>);
			if (!WalkPathFinder.TryFindWalkPath(visibleMap.mapPawns.FreeColonistsSpawned.First(), root, out list))
			{
				visibleMap.debugDrawer.FlashCell(root, 0.2f, "NOPATH", 50);
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					visibleMap.debugDrawer.FlashCell(list[i], (float)i / (float)numEntries, i.ToString(), 50);
					if (i > 0)
					{
						visibleMap.debugDrawer.FlashLine(list[i], list[i - 1], 50);
					}
				}
			}
		}
	}
}
