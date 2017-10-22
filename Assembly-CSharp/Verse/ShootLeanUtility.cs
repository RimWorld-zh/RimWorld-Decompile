using System.Collections.Generic;

namespace Verse
{
	public static class ShootLeanUtility
	{
		private static Queue<bool[]> blockedArrays = new Queue<bool[]>();

		private static bool[] GetWorkingBlockedArray()
		{
			if (ShootLeanUtility.blockedArrays.Count > 0)
			{
				return ShootLeanUtility.blockedArrays.Dequeue();
			}
			return new bool[8];
		}

		private static void ReturnWorkingBlockedArray(bool[] ar)
		{
			ShootLeanUtility.blockedArrays.Enqueue(ar);
			if (ShootLeanUtility.blockedArrays.Count > 128)
			{
				Log.ErrorOnce("Too many blocked arrays to be feasible. >128", 388121);
			}
		}

		public static void LeanShootingSourcesFromTo(IntVec3 shooterLoc, IntVec3 targetPos, Map map, List<IntVec3> listToFill)
		{
			listToFill.Clear();
			float angleFlat = (targetPos - shooterLoc).AngleFlat;
			bool flag = angleFlat > 270.0 || angleFlat < 90.0;
			bool flag2 = angleFlat > 90.0 && angleFlat < 270.0;
			bool flag3 = angleFlat > 180.0;
			bool flag4 = angleFlat < 180.0;
			bool[] workingBlockedArray = ShootLeanUtility.GetWorkingBlockedArray();
			for (int i = 0; i < 8; i++)
			{
				workingBlockedArray[i] = !(shooterLoc + GenAdj.AdjacentCells[i]).CanBeSeenOver(map);
			}
			if (!workingBlockedArray[1])
			{
				if (workingBlockedArray[0] && !workingBlockedArray[5] && flag)
				{
					goto IL_00d3;
				}
				if (workingBlockedArray[2] && !workingBlockedArray[4] && flag2)
					goto IL_00d3;
			}
			goto IL_00e7;
			IL_0134:
			if (!workingBlockedArray[2])
			{
				if (workingBlockedArray[3] && !workingBlockedArray[7] && flag3)
				{
					goto IL_016e;
				}
				if (workingBlockedArray[1] && !workingBlockedArray[4] && flag4)
					goto IL_016e;
			}
			goto IL_0182;
			IL_016e:
			listToFill.Add(shooterLoc + new IntVec3(0, 0, -1));
			goto IL_0182;
			IL_0120:
			listToFill.Add(shooterLoc + new IntVec3(-1, 0, 0));
			goto IL_0134;
			IL_01bc:
			listToFill.Add(shooterLoc + new IntVec3(0, 0, 1));
			goto IL_01d0;
			IL_0182:
			if (!workingBlockedArray[0])
			{
				if (workingBlockedArray[3] && !workingBlockedArray[6] && flag3)
				{
					goto IL_01bc;
				}
				if (workingBlockedArray[1] && !workingBlockedArray[5] && flag4)
					goto IL_01bc;
			}
			goto IL_01d0;
			IL_01d0:
			for (int j = 0; j < 4; j++)
			{
				if (!workingBlockedArray[j] && (j != 0 || flag) && (j != 1 || flag4) && (j != 2 || flag2) && (j != 3 || flag3) && (shooterLoc + GenAdj.AdjacentCells[j]).GetCover(map) != null)
				{
					listToFill.Add(shooterLoc + GenAdj.AdjacentCells[j]);
				}
			}
			ShootLeanUtility.ReturnWorkingBlockedArray(workingBlockedArray);
			return;
			IL_00d3:
			listToFill.Add(shooterLoc + new IntVec3(1, 0, 0));
			goto IL_00e7;
			IL_00e7:
			if (!workingBlockedArray[3])
			{
				if (workingBlockedArray[0] && !workingBlockedArray[6] && flag)
				{
					goto IL_0120;
				}
				if (workingBlockedArray[2] && !workingBlockedArray[7] && flag2)
					goto IL_0120;
			}
			goto IL_0134;
		}

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
				if (t.def.size.x == 1 && t.def.size.z == 1)
					return;
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
