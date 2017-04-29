using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public static class GenAdj
	{
		public static IntVec3[] CardinalDirections;

		public static IntVec3[] CardinalDirectionsAndInside;

		public static IntVec3[] CardinalDirectionsAround;

		public static IntVec3[] DiagonalDirections;

		public static IntVec3[] DiagonalDirectionsAround;

		public static IntVec3[] AdjacentCells;

		public static IntVec3[] AdjacentCellsAndInside;

		public static IntVec3[] AdjacentCellsAround;

		public static IntVec3[] AdjacentCellsAroundBottom;

		public static IntVec3[] InsideAndAdjacentCells;

		private static List<IntVec3> adjRandomOrderList;

		private static List<IntVec3> validCells;

		static GenAdj()
		{
			GenAdj.CardinalDirections = new IntVec3[4];
			GenAdj.CardinalDirectionsAndInside = new IntVec3[5];
			GenAdj.CardinalDirectionsAround = new IntVec3[4];
			GenAdj.DiagonalDirections = new IntVec3[4];
			GenAdj.DiagonalDirectionsAround = new IntVec3[4];
			GenAdj.AdjacentCells = new IntVec3[8];
			GenAdj.AdjacentCellsAndInside = new IntVec3[9];
			GenAdj.AdjacentCellsAround = new IntVec3[8];
			GenAdj.AdjacentCellsAroundBottom = new IntVec3[9];
			GenAdj.InsideAndAdjacentCells = new IntVec3[9];
			GenAdj.validCells = new List<IntVec3>();
			GenAdj.SetupAdjacencyTables();
		}

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

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsOccupiedBy(Thing t)
		{
			GenAdj.<CellsOccupiedBy>c__Iterator239 <CellsOccupiedBy>c__Iterator = new GenAdj.<CellsOccupiedBy>c__Iterator239();
			<CellsOccupiedBy>c__Iterator.t = t;
			<CellsOccupiedBy>c__Iterator.<$>t = t;
			GenAdj.<CellsOccupiedBy>c__Iterator239 expr_15 = <CellsOccupiedBy>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsOccupiedBy(IntVec3 center, Rot4 rotation, IntVec2 size)
		{
			GenAdj.<CellsOccupiedBy>c__Iterator23A <CellsOccupiedBy>c__Iterator23A = new GenAdj.<CellsOccupiedBy>c__Iterator23A();
			<CellsOccupiedBy>c__Iterator23A.center = center;
			<CellsOccupiedBy>c__Iterator23A.size = size;
			<CellsOccupiedBy>c__Iterator23A.rotation = rotation;
			<CellsOccupiedBy>c__Iterator23A.<$>center = center;
			<CellsOccupiedBy>c__Iterator23A.<$>size = size;
			<CellsOccupiedBy>c__Iterator23A.<$>rotation = rotation;
			GenAdj.<CellsOccupiedBy>c__Iterator23A expr_31 = <CellsOccupiedBy>c__Iterator23A;
			expr_31.$PC = -2;
			return expr_31;
		}

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsAdjacent8Way(TargetInfo pack)
		{
			GenAdj.<CellsAdjacent8Way>c__Iterator23B <CellsAdjacent8Way>c__Iterator23B = new GenAdj.<CellsAdjacent8Way>c__Iterator23B();
			<CellsAdjacent8Way>c__Iterator23B.pack = pack;
			<CellsAdjacent8Way>c__Iterator23B.<$>pack = pack;
			GenAdj.<CellsAdjacent8Way>c__Iterator23B expr_15 = <CellsAdjacent8Way>c__Iterator23B;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsAdjacent8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			GenAdj.<CellsAdjacent8Way>c__Iterator23C <CellsAdjacent8Way>c__Iterator23C = new GenAdj.<CellsAdjacent8Way>c__Iterator23C();
			<CellsAdjacent8Way>c__Iterator23C.thingCenter = thingCenter;
			<CellsAdjacent8Way>c__Iterator23C.thingSize = thingSize;
			<CellsAdjacent8Way>c__Iterator23C.thingRot = thingRot;
			<CellsAdjacent8Way>c__Iterator23C.<$>thingCenter = thingCenter;
			<CellsAdjacent8Way>c__Iterator23C.<$>thingSize = thingSize;
			<CellsAdjacent8Way>c__Iterator23C.<$>thingRot = thingRot;
			GenAdj.<CellsAdjacent8Way>c__Iterator23C expr_31 = <CellsAdjacent8Way>c__Iterator23C;
			expr_31.$PC = -2;
			return expr_31;
		}

		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsAdjacentCardinal(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.<CellsAdjacentCardinal>c__Iterator23D <CellsAdjacentCardinal>c__Iterator23D = new GenAdj.<CellsAdjacentCardinal>c__Iterator23D();
			<CellsAdjacentCardinal>c__Iterator23D.center = center;
			<CellsAdjacentCardinal>c__Iterator23D.size = size;
			<CellsAdjacentCardinal>c__Iterator23D.rot = rot;
			<CellsAdjacentCardinal>c__Iterator23D.<$>center = center;
			<CellsAdjacentCardinal>c__Iterator23D.<$>size = size;
			<CellsAdjacentCardinal>c__Iterator23D.<$>rot = rot;
			GenAdj.<CellsAdjacentCardinal>c__Iterator23D expr_31 = <CellsAdjacentCardinal>c__Iterator23D;
			expr_31.$PC = -2;
			return expr_31;
		}

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsAdjacentAlongEdge(IntVec3 thingCent, Rot4 thingRot, IntVec2 thingSize, LinkDirections dir)
		{
			GenAdj.<CellsAdjacentAlongEdge>c__Iterator23E <CellsAdjacentAlongEdge>c__Iterator23E = new GenAdj.<CellsAdjacentAlongEdge>c__Iterator23E();
			<CellsAdjacentAlongEdge>c__Iterator23E.thingCent = thingCent;
			<CellsAdjacentAlongEdge>c__Iterator23E.thingSize = thingSize;
			<CellsAdjacentAlongEdge>c__Iterator23E.thingRot = thingRot;
			<CellsAdjacentAlongEdge>c__Iterator23E.dir = dir;
			<CellsAdjacentAlongEdge>c__Iterator23E.<$>thingCent = thingCent;
			<CellsAdjacentAlongEdge>c__Iterator23E.<$>thingSize = thingSize;
			<CellsAdjacentAlongEdge>c__Iterator23E.<$>thingRot = thingRot;
			<CellsAdjacentAlongEdge>c__Iterator23E.<$>dir = dir;
			GenAdj.<CellsAdjacentAlongEdge>c__Iterator23E expr_3F = <CellsAdjacentAlongEdge>c__Iterator23E;
			expr_3F.$PC = -2;
			return expr_3F;
		}

		[DebuggerHidden]
		public static IEnumerable<IntVec3> CellsAdjacent8WayAndInside(this Thing thing)
		{
			GenAdj.<CellsAdjacent8WayAndInside>c__Iterator23F <CellsAdjacent8WayAndInside>c__Iterator23F = new GenAdj.<CellsAdjacent8WayAndInside>c__Iterator23F();
			<CellsAdjacent8WayAndInside>c__Iterator23F.thing = thing;
			<CellsAdjacent8WayAndInside>c__Iterator23F.<$>thing = thing;
			GenAdj.<CellsAdjacent8WayAndInside>c__Iterator23F expr_15 = <CellsAdjacent8WayAndInside>c__Iterator23F;
			expr_15.$PC = -2;
			return expr_15;
		}

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

		private static void GetAdjacentCorners(CellRect rect, out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(rect.minX - 1, 0, rect.minZ - 1);
			TL = new IntVec3(rect.minX - 1, 0, rect.maxZ + 1);
			TR = new IntVec3(rect.maxX + 1, 0, rect.maxZ + 1);
			BR = new IntVec3(rect.maxX + 1, 0, rect.minZ - 1);
		}

		public static IntVec3 RandomAdjacentCell8Way(this IntVec3 root)
		{
			return root + GenAdj.AdjacentCells[Rand.RangeInclusive(0, 7)];
		}

		public static IntVec3 RandomAdjacentCellCardinal(this IntVec3 root)
		{
			return root + GenAdj.CardinalDirections[Rand.RangeInclusive(0, 3)];
		}

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

		public static bool TryFindRandomAdjacentCell8WayWithRoomGroup(Thing t, out IntVec3 result)
		{
			return GenAdj.TryFindRandomAdjacentCell8WayWithRoomGroup(t.Position, t.Rotation, t.def.size, t.Map, out result);
		}

		public static bool TryFindRandomAdjacentCell8WayWithRoomGroup(IntVec3 center, Rot4 rot, IntVec2 size, Map map, out IntVec3 result)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			GenAdj.validCells.Clear();
			foreach (IntVec3 current in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (current.InBounds(map) && current.GetRoomGroup(map) != null)
				{
					GenAdj.validCells.Add(current);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

		public static bool AdjacentTo8WayOrInside(this IntVec3 me, LocalTargetInfo other)
		{
			if (other.HasThing)
			{
				return me.AdjacentTo8WayOrInside(other.Thing);
			}
			return me.AdjacentTo8WayOrInside(other.Cell);
		}

		public static bool AdjacentTo8Way(this IntVec3 me, IntVec3 other)
		{
			int num = me.x - other.x;
			int num2 = me.z - other.z;
			if (num == 0 && num2 == 0)
			{
				return false;
			}
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

		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			CellRect cellRect = t1.OccupiedRect().ExpandedBy(1);
			CellRect cellRect2 = t2.OccupiedRect();
			int minX = cellRect.minX;
			int maxX = cellRect.maxX;
			int minZ = cellRect.minZ;
			int maxZ = cellRect.maxZ;
			int i = minX;
			int j = minZ;
			while (i <= maxX)
			{
				if (cellRect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
				i++;
			}
			i--;
			for (j++; j <= maxZ; j++)
			{
				if (cellRect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			j--;
			for (i--; i >= minX; i--)
			{
				if (cellRect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			i++;
			for (j--; j > minZ; j--)
			{
				if (cellRect2.Contains(new IntVec3(i, 0, j)) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != minZ) && (i != maxX || j != maxZ))
				{
					return true;
				}
			}
			return false;
		}

		public static bool AdjacentTo8WayOrInside(this IntVec3 root, Thing t)
		{
			return root.AdjacentTo8WayOrInside(t.Position, t.Rotation, t.def.size);
		}

		public static bool AdjacentTo8WayOrInside(this IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2 - 1;
			int num2 = center.z - (size.z - 1) / 2 - 1;
			int num3 = num + size.x + 1;
			int num4 = num2 + size.z + 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		public static bool IsInside(this IntVec3 root, Thing t)
		{
			return GenAdj.IsInside(root, t.Position, t.Rotation, t.def.size);
		}

		public static bool IsInside(IntVec3 root, IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			int num = center.x - (size.x - 1) / 2;
			int num2 = center.z - (size.z - 1) / 2;
			int num3 = num + size.x - 1;
			int num4 = num2 + size.z - 1;
			return root.x >= num && root.x <= num3 && root.z >= num2 && root.z <= num4;
		}

		public static CellRect OccupiedRect(this Thing t)
		{
			return GenAdj.OccupiedRect(t.Position, t.Rotation, t.def.size);
		}

		public static CellRect OccupiedRect(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			GenAdj.AdjustForRotation(ref center, ref size, rot);
			return new CellRect(center.x - (size.x - 1) / 2, center.z - (size.z - 1) / 2, size.x, size.z);
		}

		public static void AdjustForRotation(ref IntVec3 center, ref IntVec2 size, Rot4 rot)
		{
			if (size.x == 1 && size.z == 1)
			{
				return;
			}
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
