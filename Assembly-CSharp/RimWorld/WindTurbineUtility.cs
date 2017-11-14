using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class WindTurbineUtility
	{
		public static IEnumerable<IntVec3> CalculateWindCells(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			CellRect rectA = default(CellRect);
			CellRect rectB = default(CellRect);
			int offset = 0;
			int neDist;
			int swDist;
			if (rot == Rot4.North || rot == Rot4.East)
			{
				neDist = 9;
				swDist = 5;
			}
			else
			{
				neDist = 5;
				swDist = 9;
				offset = -1;
			}
			if (rot.IsHorizontal)
			{
				rectA.minX = center.x + 2 + offset;
				rectA.maxX = center.x + 2 + neDist + offset;
				rectB.minX = center.x - 1 - swDist + offset;
				rectB.maxX = center.x - 1 + offset;
				rectB.minZ = (rectA.minZ = center.z - 2);
				rectB.maxZ = (rectA.maxZ = center.z + 2);
			}
			else
			{
				rectA.minZ = center.z + 2 + offset;
				rectA.maxZ = center.z + 2 + neDist + offset;
				rectB.minZ = center.z - 1 - swDist + offset;
				rectB.maxZ = center.z - 1 + offset;
				rectB.minX = (rectA.minX = center.x - 2);
				rectB.maxX = (rectA.maxX = center.x + 2);
			}
			for (int z2 = rectA.minZ; z2 <= rectA.maxZ; z2++)
			{
				int x = rectA.minX;
				if (x <= rectA.maxX)
				{
					yield return new IntVec3(x, 0, z2);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			int z = rectB.minZ;
			int x2;
			while (true)
			{
				if (z <= rectB.maxZ)
				{
					x2 = rectB.minX;
					if (x2 <= rectB.maxX)
						break;
					z++;
					continue;
				}
				yield break;
			}
			yield return new IntVec3(x2, 0, z);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
