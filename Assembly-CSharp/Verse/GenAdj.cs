using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F32 RID: 3890
	public static class GenAdj
	{
		// Token: 0x04003DC2 RID: 15810
		public static IntVec3[] CardinalDirections = new IntVec3[4];

		// Token: 0x04003DC3 RID: 15811
		public static IntVec3[] CardinalDirectionsAndInside = new IntVec3[5];

		// Token: 0x04003DC4 RID: 15812
		public static IntVec3[] CardinalDirectionsAround = new IntVec3[4];

		// Token: 0x04003DC5 RID: 15813
		public static IntVec3[] DiagonalDirections = new IntVec3[4];

		// Token: 0x04003DC6 RID: 15814
		public static IntVec3[] DiagonalDirectionsAround = new IntVec3[4];

		// Token: 0x04003DC7 RID: 15815
		public static IntVec3[] AdjacentCells = new IntVec3[8];

		// Token: 0x04003DC8 RID: 15816
		public static IntVec3[] AdjacentCellsAndInside = new IntVec3[9];

		// Token: 0x04003DC9 RID: 15817
		public static IntVec3[] AdjacentCellsAround = new IntVec3[8];

		// Token: 0x04003DCA RID: 15818
		public static IntVec3[] AdjacentCellsAroundBottom = new IntVec3[9];

		// Token: 0x04003DCB RID: 15819
		private static List<IntVec3> adjRandomOrderList;

		// Token: 0x04003DCC RID: 15820
		private static List<IntVec3> validCells = new List<IntVec3>();

		// Token: 0x06005D85 RID: 23941 RVA: 0x002F6BB4 File Offset: 0x002F4FB4
		static GenAdj()
		{
			GenAdj.SetupAdjacencyTables();
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x002F6C38 File Offset: 0x002F5038
		private static void SetupAdjacencyTables()
		{
			GenAdj.CardinalDirections[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirections[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirections[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirections[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAndInside[4] = new IntVec3(0, 0, 0);
			GenAdj.CardinalDirectionsAround[0] = new IntVec3(0, 0, -1);
			GenAdj.CardinalDirectionsAround[1] = new IntVec3(-1, 0, 0);
			GenAdj.CardinalDirectionsAround[2] = new IntVec3(0, 0, 1);
			GenAdj.CardinalDirectionsAround[3] = new IntVec3(1, 0, 0);
			GenAdj.DiagonalDirections[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirections[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirections[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirections[3] = new IntVec3(1, 0, -1);
			GenAdj.DiagonalDirectionsAround[0] = new IntVec3(-1, 0, -1);
			GenAdj.DiagonalDirectionsAround[1] = new IntVec3(-1, 0, 1);
			GenAdj.DiagonalDirectionsAround[2] = new IntVec3(1, 0, 1);
			GenAdj.DiagonalDirectionsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCells[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCells[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCells[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCells[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCells[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCells[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCells[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAndInside[1] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAndInside[2] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAndInside[3] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAndInside[4] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAndInside[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAndInside[6] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAndInside[7] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAndInside[8] = new IntVec3(0, 0, 0);
			GenAdj.AdjacentCellsAround[0] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAround[1] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAround[2] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAround[3] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAround[4] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAround[5] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAround[6] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAround[7] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[0] = new IntVec3(0, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[1] = new IntVec3(-1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[2] = new IntVec3(-1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[3] = new IntVec3(-1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[4] = new IntVec3(0, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[5] = new IntVec3(1, 0, 1);
			GenAdj.AdjacentCellsAroundBottom[6] = new IntVec3(1, 0, 0);
			GenAdj.AdjacentCellsAroundBottom[7] = new IntVec3(1, 0, -1);
			GenAdj.AdjacentCellsAroundBottom[8] = new IntVec3(0, 0, 0);
		}

		// Token: 0x06005D87 RID: 23943 RVA: 0x002F7170 File Offset: 0x002F5570
		public static List<IntVec3> AdjacentCells8WayRandomized()
		{
			if (GenAdj.adjRandomOrderList == null)
			{
				GenAdj.adjRandomOrderList = new List<IntVec3>();
				for (int i = 0; i < 8; i++)
				{
					GenAdj.adjRandomOrderList.Add(GenAdj.AdjacentCells[i]);
				}
			}
			GenAdj.adjRandomOrderList.Shuffle<IntVec3>();
			return GenAdj.adjRandomOrderList;
		}

		// Token: 0x06005D88 RID: 23944 RVA: 0x002F71D8 File Offset: 0x002F55D8
		public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				yield return t.Position;
			}
			else
			{
				foreach (IntVec3 c in GenAdj.CellsOccupiedBy(t.Position, t.Rotation, t.def.size))
				{
					yield return c;
				}
			}
			yield break;
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x002F7204 File Offset: 0x002F5604
		public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rotation);
			int minX = center.x - (size.x - 1) / 2;
			int minZ = center.z - (size.z - 1) / 2;
			int maxX = minX + size.x - 1;
			int maxZ = minZ + size.z - 1;
			for (int i = minX; i <= maxX; i++)
			{
				for (int j = minZ; j <= maxZ; j++)
				{
					yield return new IntVec3(i, 0, j);
				}
			}
			yield break;
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x002F724C File Offset: 0x002F564C
		public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
		{
			if (pack.HasThing)
			{
				foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(pack.Thing))
				{
					yield return c;
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					yield return pack.Cell + GenAdj.AdjacentCells[i];
				}
			}
			yield break;
		}

		// Token: 0x06005D8B RID: 23947 RVA: 0x002F7278 File Offset: 0x002F5678
		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005D8C RID: 23948 RVA: 0x002F72AC File Offset: 0x002F56AC
		public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int minX = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int minZ = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int maxZ = minZ + thingSize.z + 1;
			IntVec3 cur = new IntVec3(minX - 1, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX);
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ);
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX);
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x002F72F4 File Offset: 0x002F56F4
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x002F7328 File Offset: 0x002F5728
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int minX = center.x - (size.x - 1) / 2 - 1;
			int maxX = minX + size.x + 1;
			int minZ = center.z - (size.z - 1) / 2 - 1;
			int maxZ = minZ + size.z + 1;
			IntVec3 cur = new IntVec3(minX, 0, minZ);
			do
			{
				cur.x++;
				yield return cur;
			}
			while (cur.x < maxX - 1);
			cur.x++;
			do
			{
				cur.z++;
				yield return cur;
			}
			while (cur.z < maxZ - 1);
			cur.z++;
			do
			{
				cur.x--;
				yield return cur;
			}
			while (cur.x > minX + 1);
			cur.x--;
			do
			{
				cur.z--;
				yield return cur;
			}
			while (cur.z > minZ + 1);
			yield break;
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x002F7370 File Offset: 0x002F5770
		public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 thingCent, Rot4 thingRot, IntVec2 thingSize, LinkDirections dir)
		{
			GenAdj.AdjustForRotation(ref thingCent, ref thingSize, thingRot);
			int minX = thingCent.x - (thingSize.x - 1) / 2 - 1;
			int minZ = thingCent.z - (thingSize.z - 1) / 2 - 1;
			int maxX = minX + thingSize.x + 1;
			int maxZ = minZ + thingSize.z + 1;
			if (dir == LinkDirections.Down)
			{
				for (int x = minX; x <= maxX; x++)
				{
					yield return new IntVec3(x, thingCent.y, minZ - 1);
				}
			}
			if (dir == LinkDirections.Up)
			{
				for (int x2 = minX; x2 <= maxX; x2++)
				{
					yield return new IntVec3(x2, thingCent.y, maxZ + 1);
				}
			}
			if (dir == LinkDirections.Left)
			{
				for (int z = minZ; z <= maxZ; z++)
				{
					yield return new IntVec3(minX - 1, thingCent.y, z);
				}
			}
			if (dir == LinkDirections.Right)
			{
				for (int z2 = minZ; z2 <= maxZ; z2++)
				{
					yield return new IntVec3(maxX + 1, thingCent.y, z2);
				}
			}
			yield break;
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x002F73C0 File Offset: 0x002F57C0
		public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
		{
			IntVec3 center = thing.Position;
			IntVec2 size = thing.def.size;
			Rot4 rotation = thing.Rotation;
			GenAdj.AdjustForRotation(ref center, ref size, rotation);
			int minX = center.x - (size.x - 1) / 2 - 1;
			int minZ = center.z - (size.z - 1) / 2 - 1;
			int maxX = minX + size.x + 1;
			int maxZ = minZ + size.z + 1;
			for (int i = minX; i <= maxX; i++)
			{
				for (int j = minZ; j <= maxZ; j++)
				{
					yield return new IntVec3(i, 0, j);
				}
			}
			yield break;
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x002F73EA File Offset: 0x002F57EA
		public static void GetAdjacentCorners(LocalTargetInfo target, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			if (target.HasThing)
			{
				GenAdj.GetAdjacentCorners(target.Thing.OccupiedRect(), out BL, out TL, out TR, out BR);
			}
			else
			{
				GenAdj.GetAdjacentCorners(CellRect.SingleCell(target.Cell), out BL, out TL, out TR, out BR);
			}
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x002F742C File Offset: 0x002F582C
		private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
			TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
			TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
			BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x002F74A0 File Offset: 0x002F58A0
		public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
		{
			return root + GenAdj.AdjacentCells[Rand.RangeInclusive(0, 7)];
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x002F74D4 File Offset: 0x002F58D4
		public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
		{
			return root + GenAdj.CardinalDirections[Rand.RangeInclusive(0, 3)];
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x002F7508 File Offset: 0x002F5908
		public static IntVec3 RandomAdjacentCell8Way(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect cellRect2 = cellRect.ExpandedBy(1);
			IntVec3 randomCell;
			do
			{
				randomCell = cellRect2.RandomCell;
			}
			while (cellRect.Contains(randomCell));
			return randomCell;
		}

		// Token: 0x06005D96 RID: 23958 RVA: 0x002F754C File Offset: 0x002F594C
		public static IntVec3 RandomAdjacentCellCardinal(this Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			IntVec3 randomCell = cellRect.RandomCell;
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					randomCell.x = cellRect.minX - 1;
				}
				else
				{
					randomCell.x = cellRect.maxX + 1;
				}
			}
			else if (Rand.Value < 0.5f)
			{
				randomCell.z = cellRect.minZ - 1;
			}
			else
			{
				randomCell.z = cellRect.maxZ + 1;
			}
			return randomCell;
		}

		// Token: 0x06005D97 RID: 23959 RVA: 0x002F75F4 File Offset: 0x002F59F4
		public static bool TryFindRandomAdjacentCell8WayWithRoomGroup(Thing t, out IntVec3 result)
		{
			return GenAdj.TryFindRandomAdjacentCell8WayWithRoomGroup(t.Position, t.Rotation, t.def.size, t.Map, out result);
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x002F762C File Offset: 0x002F5A2C
		public static bool TryFindRandomAdjacentCell8WayWithRoomGroup(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			GenAdj.validCells.Clear();
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (intVec.InBounds(map) && intVec.GetRoomGroup(map) != null)
				{
					GenAdj.validCells.Add(intVec);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x002F76D0 File Offset: 0x002F5AD0
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
		{
			bool result;
			if (other.HasThing)
			{
				result = me.AdjacentTo8WayOrInside(other.Thing);
			}
			else
			{
				result = me.AdjacentTo8WayOrInside(other.Cell);
			}
			return result;
		}

		// Token: 0x06005D9A RID: 23962 RVA: 0x002F7714 File Offset: 0x002F5B14
		public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			bool result;
			if (num == 0 && num2 == 0)
			{
				result = false;
			}
			else
			{
				if (num < 0)
				{
					num *= -1;
				}
				if (num2 < 0)
				{
					num2 *= -1;
				}
				result = (num <= 1 && num2 <= 1);
			}
			return result;
		}

		// Token: 0x06005D9B RID: 23963 RVA: 0x002F7784 File Offset: 0x002F5B84
		public static bool AdjacentTo8WayOrInside(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num < 0)
			{
				num *= -1;
			}
			if (num2 < 0)
			{
				num2 *= -1;
			}
			return num <= 1 && num2 <= 1;
		}

		// Token: 0x06005D9C RID: 23964 RVA: 0x002F77E0 File Offset: 0x002F5BE0
		public static bool IsAdjacentToCardinalOrInside(this IntVec3 me, CellRect other)
		{
			bool result;
			if (other.IsEmpty)
			{
				result = false;
			}
			else
			{
				CellRect cellRect = other.ExpandedBy(1);
				result = (cellRect.Contains(me) && !cellRect.IsCorner(me));
			}
			return result;
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x002F782C File Offset: 0x002F5C2C
		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			return GenAdj.IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x002F7854 File Offset: 0x002F5C54
		public static bool IsAdjacentToCardinalOrInside(CellRect rect1, CellRect rect2)
		{
			bool result;
			if (rect1.IsEmpty || rect2.IsEmpty)
			{
				result = false;
			}
			else
			{
				CellRect cellRect = rect1.ExpandedBy(1);
				int minX = cellRect.minX;
				int maxX = cellRect.maxX;
				int minZ = cellRect.minZ;
				int maxZ = cellRect.maxZ;
				int i = minX;
				int j = minZ;
				while (i <= maxX)
				{
					if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
					{
						return true;
					}
					i++;
				}
				i--;
				for (j++; j <= maxZ; j++)
				{
					if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
					{
						return true;
					}
				}
				j--;
				for (i--; i >= minX; i--)
				{
					if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
					{
						return true;
					}
				}
				i++;
				for (j--; j > minZ; j--)
				{
					if (rect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x002F7AC0 File Offset: 0x002F5EC0
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
		{
			return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x002F7AF4 File Offset: 0x002F5EF4
		public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2 - 1;
			int num2 = center.z - (size.z - 1) / 2 - 1;
			int num3 = num + size.x + 1;
			int num4 = num2 + size.z + 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x002F7B98 File Offset: 0x002F5F98
		public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
		{
			return GenAdj.AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x002F7BC0 File Offset: 0x002F5FC0
		public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && rect1.ExpandedBy(1).Overlaps(rect2);
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x002F7C08 File Offset: 0x002F6008
		public static bool IsInside(this IntVec3 root, Thing t)
		{
			return GenAdj.IsInside(root, t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x002F7C3C File Offset: 0x002F603C
		public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2;
			int num2 = center.z - (size.z - 1) / 2;
			int num3 = num + size.x - 1;
			int num4 = num2 + size.z - 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x002F7CDC File Offset: 0x002F60DC
		public static CellRect OccupiedRect(this Thing t)
		{
			return GenAdj.OccupiedRect(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DA6 RID: 23974 RVA: 0x002F7D10 File Offset: 0x002F6110
		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x002F7D68 File Offset: 0x002F6168
		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
		{
			if (size.x != 1 || size.z != 1)
			{
				if (rot.IsHorizontal)
				{
					int x = size.x;
					size.x = size.z;
					size.z = x;
				}
				switch (rot.AsInt)
				{
				case 1:
					if (size.z % 2 == 0)
					{
						center.z--;
					}
					break;
				case 2:
					if (size.x % 2 == 0)
					{
						center.x--;
					}
					if (size.z % 2 == 0)
					{
						center.z--;
					}
					break;
				case 3:
					if (size.x % 2 == 0)
					{
						center.x--;
					}
					break;
				}
			}
		}
	}
}
