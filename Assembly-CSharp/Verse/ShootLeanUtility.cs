using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB7 RID: 4023
	public static class ShootLeanUtility
	{
		// Token: 0x04003F99 RID: 16281
		private static Queue<bool[]> blockedArrays = new Queue<bool[]>();

		// Token: 0x06006148 RID: 24904 RVA: 0x003126E4 File Offset: 0x00310AE4
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

		// Token: 0x06006149 RID: 24905 RVA: 0x0031271F File Offset: 0x00310B1F
		private static void ReturnWorkingBlockedArray(bool[] ar)
		{
			ShootLeanUtility.blockedArrays.Enqueue(ar);
			if (ShootLeanUtility.blockedArrays.Count > 128)
			{
				Log.ErrorOnce("Too many blocked arrays to be feasible. >128", 388121, false);
			}
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x00312754 File Offset: 0x00310B54
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

		// Token: 0x0600614B RID: 24907 RVA: 0x003129F8 File Offset: 0x00310DF8
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
