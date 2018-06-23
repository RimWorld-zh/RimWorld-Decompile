using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB2 RID: 4018
	public static class ShootLeanUtility
	{
		// Token: 0x04003F8E RID: 16270
		private static Queue<bool[]> blockedArrays = new Queue<bool[]>();

		// Token: 0x0600613E RID: 24894 RVA: 0x00311E20 File Offset: 0x00310220
		private static bool[] GetWorkingBlockedArray()
		{
			bool[] result;
			if (ShootLeanUtility.blockedArrays.Count > 0)
			{
				result = ShootLeanUtility.blockedArrays.Dequeue();
			}
			else
			{
				result = new bool[8];
			}
			return result;
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x00311E5B File Offset: 0x0031025B
		private static void ReturnWorkingBlockedArray(bool[] ar)
		{
			ShootLeanUtility.blockedArrays.Enqueue(ar);
			if (ShootLeanUtility.blockedArrays.Count > 128)
			{
				Log.ErrorOnce("Too many blocked arrays to be feasible. >128", 388121, false);
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x00311E90 File Offset: 0x00310290
		public static void LeanShootingSourcesFromTo(IntVec3 shooterLoc, IntVec3 targetPos, Map map, List<IntVec3> listToFill)
		{
			listToFill.Clear();
			float angleFlat = (targetPos - shooterLoc).AngleFlat;
			bool flag = angleFlat > 270f || angleFlat < 90f;
			bool flag2 = angleFlat > 90f && angleFlat < 270f;
			bool flag3 = angleFlat > 180f;
			bool flag4 = angleFlat < 180f;
			bool[] workingBlockedArray = ShootLeanUtility.GetWorkingBlockedArray();
			for (int i = 0; i < 8; i++)
			{
				workingBlockedArray[i] = !(shooterLoc + GenAdj.AdjacentCells[i]).CanBeSeenOver(map);
			}
			if (!workingBlockedArray[1])
			{
				if ((workingBlockedArray[0] && !workingBlockedArray[5] && flag) || (workingBlockedArray[2] && !workingBlockedArray[4] && flag2))
				{
					listToFill.Add(shooterLoc + new IntVec3(1, 0, 0));
				}
			}
			if (!workingBlockedArray[3])
			{
				if ((workingBlockedArray[0] && !workingBlockedArray[6] && flag) || (workingBlockedArray[2] && !workingBlockedArray[7] && flag2))
				{
					listToFill.Add(shooterLoc + new IntVec3(-1, 0, 0));
				}
			}
			if (!workingBlockedArray[2])
			{
				if ((workingBlockedArray[3] && !workingBlockedArray[7] && flag3) || (workingBlockedArray[1] && !workingBlockedArray[4] && flag4))
				{
					listToFill.Add(shooterLoc + new IntVec3(0, 0, -1));
				}
			}
			if (!workingBlockedArray[0])
			{
				if ((workingBlockedArray[3] && !workingBlockedArray[6] && flag3) || (workingBlockedArray[1] && !workingBlockedArray[5] && flag4))
				{
					listToFill.Add(shooterLoc + new IntVec3(0, 0, 1));
				}
			}
			int j = 0;
			while (j < 4)
			{
				if (!workingBlockedArray[j])
				{
					if (j != 0 || flag)
					{
						if (j != 1 || flag4)
						{
							if (j != 2 || flag2)
							{
								if (j != 3 || flag3)
								{
									if ((shooterLoc + GenAdj.AdjacentCells[j]).GetCover(map) != null)
									{
										listToFill.Add(shooterLoc + GenAdj.AdjacentCells[j]);
									}
								}
							}
						}
					}
				}
				IL_27F:
				j++;
				continue;
				goto IL_27F;
			}
			ShootLeanUtility.ReturnWorkingBlockedArray(workingBlockedArray);
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x00312134 File Offset: 0x00310534
		public static void CalcShootableCellsOf(List<IntVec3> outCells, Thing t)
		{
			outCells.Clear();
			if (t is Pawn)
			{
				outCells.Add(t.Position);
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = t.Position + GenAdj.CardinalDirections[i];
					if (intVec.CanBeSeenOver(t.Map))
					{
						outCells.Add(intVec);
					}
				}
			}
			else
			{
				outCells.Add(t.Position);
				if (t.def.size.x != 1 || t.def.size.z != 1)
				{
					CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
					while (!iterator.Done())
					{
						if (iterator.Current != t.Position)
						{
							outCells.Add(iterator.Current);
						}
						iterator.MoveNext();
					}
				}
			}
		}
	}
}
