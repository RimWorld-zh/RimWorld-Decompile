using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public static class GenAdj
	{
		public static IntVec3[] CardinalDirections = new IntVec3[4];

		public static IntVec3[] CardinalDirectionsAndInside = new IntVec3[5];

		public static IntVec3[] CardinalDirectionsAround = new IntVec3[4];

		public static IntVec3[] DiagonalDirections = new IntVec3[4];

		public static IntVec3[] DiagonalDirectionsAround = new IntVec3[4];

		public static IntVec3[] AdjacentCells = new IntVec3[8];

		public static IntVec3[] AdjacentCellsAndInside = new IntVec3[9];

		public static IntVec3[] AdjacentCellsAround = new IntVec3[8];

		public static IntVec3[] AdjacentCellsAroundBottom = new IntVec3[9];

		private static List<IntVec3> adjRandomOrderList;

		private static List<IntVec3> validCells = new List<IntVec3>();

		static GenAdj()
		{
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

		public static IEnumerable<IntVec3> CellsAdjacent8Way(Thing t)
		{
			return GenAdj.CellsAdjacent8Way(t.Position, t.Rotation, t.def.size);
		}

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

		public static IEnumerable<IntVec3> CellsAdjacentCardinal(Thing t)
		{
			return GenAdj.CellsAdjacentCardinal(t.Position, t.Rotation, t.def.size);
		}

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
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(center, rot, size))
			{
				if (intVec.InBounds(map) && intVec.GetRoomGroup(map) != null)
				{
					GenAdj.validCells.Add(intVec);
				}
			}
			return GenAdj.validCells.TryRandomElement(out result);
		}

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

		public static bool IsAdjacentToCardinalOrInside(this Thing t1, Thing t2)
		{
			return GenAdj.IsAdjacentToCardinalOrInside(t1.OccupiedRect(), t2.OccupiedRect());
		}

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

		public static bool AdjacentTo8WayOrInside(this Thing a, Thing b)
		{
			return GenAdj.AdjacentTo8WayOrInside(a.OccupiedRect(), b.OccupiedRect());
		}

		public static bool AdjacentTo8WayOrInside(CellRect rect1, CellRect rect2)
		{
			return !rect1.IsEmpty && !rect2.IsEmpty && rect1.ExpandedBy(1).Overlaps(rect2);
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

		[CompilerGenerated]
		private sealed class <CellsOccupiedBy>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Thing t;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__1;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CellsOccupiedBy>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (t.def.size.x == 1 && t.def.size.z == 1)
					{
						this.$current = t.Position;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					enumerator = GenAdj.CellsOccupiedBy(t.Position, t.Rotation, t.def.size).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					goto IL_13C;
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IL_13C:
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsOccupiedBy>c__Iterator0 <CellsOccupiedBy>c__Iterator = new GenAdj.<CellsOccupiedBy>c__Iterator0();
				<CellsOccupiedBy>c__Iterator.t = t;
				return <CellsOccupiedBy>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CellsOccupiedBy>c__Iterator1 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IntVec3 center;

			internal IntVec2 size;

			internal Rot4 rotation;

			internal int <minX>__0;

			internal int <minZ>__0;

			internal int <maxX>__0;

			internal int <maxZ>__0;

			internal int <i>__1;

			internal int <j>__2;

			internal IntVec3 $current;

			internal bool $disposing;

			internal IntVec3 <$>center;

			internal IntVec2 <$>size;

			internal int $PC;

			[DebuggerHidden]
			public <CellsOccupiedBy>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					GenAdj.AdjustForRotation(ref center, ref size, rotation);
					minX = center.x - (size.x - 1) / 2;
					minZ = center.z - (size.z - 1) / 2;
					maxX = minX + size.x - 1;
					maxZ = minZ + size.z - 1;
					i = minX;
					goto IL_12E;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_10E:
				if (j <= maxZ)
				{
					this.$current = new IntVec3(i, 0, j);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_12E:
				if (i <= maxX)
				{
					j = minZ;
					goto IL_10E;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsOccupiedBy>c__Iterator1 <CellsOccupiedBy>c__Iterator = new GenAdj.<CellsOccupiedBy>c__Iterator1();
				<CellsOccupiedBy>c__Iterator.center = center;
				<CellsOccupiedBy>c__Iterator.size = size;
				<CellsOccupiedBy>c__Iterator.rotation = rotation;
				return <CellsOccupiedBy>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CellsAdjacent8Way>c__Iterator2 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal TargetInfo pack;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__1;

			internal int <i>__2;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CellsAdjacent8Way>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (!pack.HasThing)
					{
						i = 0;
						goto IL_130;
					}
					enumerator = GenAdj.CellsAdjacent8Way(pack.Thing).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					i++;
					goto IL_130;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				goto IL_13D;
				IL_130:
				if (i < 8)
				{
					this.$current = pack.Cell + GenAdj.AdjacentCells[i];
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_13D:
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsAdjacent8Way>c__Iterator2 <CellsAdjacent8Way>c__Iterator = new GenAdj.<CellsAdjacent8Way>c__Iterator2();
				<CellsAdjacent8Way>c__Iterator.pack = pack;
				return <CellsAdjacent8Way>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CellsAdjacent8Way>c__Iterator3 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IntVec3 thingCenter;

			internal IntVec2 thingSize;

			internal Rot4 thingRot;

			internal int <minX>__0;

			internal int <maxX>__0;

			internal int <minZ>__0;

			internal int <maxZ>__0;

			internal IntVec3 <cur>__0;

			internal IntVec3 $current;

			internal bool $disposing;

			internal IntVec3 <$>thingCenter;

			internal IntVec2 <$>thingSize;

			internal int $PC;

			[DebuggerHidden]
			public <CellsAdjacent8Way>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
					minX = thingCenter.x - (thingSize.x - 1) / 2 - 1;
					maxX = minX + thingSize.x + 1;
					minZ = thingCenter.z - (thingSize.z - 1) / 2 - 1;
					maxZ = minZ + thingSize.z + 1;
					cur = new IntVec3(minX - 1, 0, minZ);
					break;
				case 1u:
					if (cur.x >= maxX)
					{
						goto IL_124;
					}
					break;
				case 2u:
					if (cur.z >= maxZ)
					{
						goto IL_16F;
					}
					goto IL_124;
				case 3u:
					if (cur.x <= minX)
					{
						goto IL_1BA;
					}
					goto IL_16F;
				case 4u:
					if (cur.z <= minZ + 1)
					{
						this.$PC = -1;
						return false;
					}
					goto IL_1BA;
				default:
					return false;
				}
				cur.x++;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				IL_124:
				cur.z++;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_16F:
				cur.x--;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
				IL_1BA:
				cur.z--;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsAdjacent8Way>c__Iterator3 <CellsAdjacent8Way>c__Iterator = new GenAdj.<CellsAdjacent8Way>c__Iterator3();
				<CellsAdjacent8Way>c__Iterator.thingCenter = thingCenter;
				<CellsAdjacent8Way>c__Iterator.thingSize = thingSize;
				<CellsAdjacent8Way>c__Iterator.thingRot = thingRot;
				return <CellsAdjacent8Way>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CellsAdjacentCardinal>c__Iterator4 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IntVec3 center;

			internal IntVec2 size;

			internal Rot4 rot;

			internal int <minX>__0;

			internal int <maxX>__0;

			internal int <minZ>__0;

			internal int <maxZ>__0;

			internal IntVec3 <cur>__0;

			internal IntVec3 $current;

			internal bool $disposing;

			internal IntVec3 <$>center;

			internal IntVec2 <$>size;

			internal int $PC;

			[DebuggerHidden]
			public <CellsAdjacentCardinal>c__Iterator4()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					GenAdj.AdjustForRotation(ref center, ref size, rot);
					minX = center.x - (size.x - 1) / 2 - 1;
					maxX = minX + size.x + 1;
					minZ = center.z - (size.z - 1) / 2 - 1;
					maxZ = minZ + size.z + 1;
					cur = new IntVec3(minX, 0, minZ);
					break;
				case 1u:
					if (cur.x >= maxX - 1)
					{
						cur.x++;
						goto IL_137;
					}
					break;
				case 2u:
					if (cur.z >= maxZ - 1)
					{
						cur.z++;
						goto IL_197;
					}
					goto IL_137;
				case 3u:
					if (cur.x <= minX + 1)
					{
						cur.x--;
						goto IL_1F7;
					}
					goto IL_197;
				case 4u:
					if (cur.z <= minZ + 1)
					{
						this.$PC = -1;
						return false;
					}
					goto IL_1F7;
				default:
					return false;
				}
				cur.x++;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				IL_137:
				cur.z++;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_197:
				cur.x--;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
				IL_1F7:
				cur.z--;
				this.$current = cur;
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsAdjacentCardinal>c__Iterator4 <CellsAdjacentCardinal>c__Iterator = new GenAdj.<CellsAdjacentCardinal>c__Iterator4();
				<CellsAdjacentCardinal>c__Iterator.center = center;
				<CellsAdjacentCardinal>c__Iterator.size = size;
				<CellsAdjacentCardinal>c__Iterator.rot = rot;
				return <CellsAdjacentCardinal>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CellsAdjacentAlongEdge>c__Iterator5 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IntVec3 thingCent;

			internal IntVec2 thingSize;

			internal Rot4 thingRot;

			internal int <minX>__0;

			internal int <minZ>__0;

			internal int <maxX>__0;

			internal int <maxZ>__0;

			internal LinkDirections dir;

			internal int <x>__1;

			internal int <x>__2;

			internal int <z>__3;

			internal int <z>__4;

			internal IntVec3 $current;

			internal bool $disposing;

			internal IntVec3 <$>thingCent;

			internal IntVec2 <$>thingSize;

			internal int $PC;

			[DebuggerHidden]
			public <CellsAdjacentAlongEdge>c__Iterator5()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					GenAdj.AdjustForRotation(ref thingCent, ref thingSize, thingRot);
					minX = thingCent.x - (thingSize.x - 1) / 2 - 1;
					minZ = thingCent.z - (thingSize.z - 1) / 2 - 1;
					maxX = minX + thingSize.x + 1;
					maxZ = minZ + thingSize.z + 1;
					if (dir != LinkDirections.Down)
					{
						goto IL_137;
					}
					x = minX;
					break;
				case 1u:
					x++;
					break;
				case 2u:
					x2++;
					goto IL_19D;
				case 3u:
					z++;
					goto IL_215;
				case 4u:
					z2++;
					goto IL_28D;
				default:
					return false;
				}
				if (x <= maxX)
				{
					this.$current = new IntVec3(x, thingCent.y, minZ - 1);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_137:
				if (dir != LinkDirections.Up)
				{
					goto IL_1AF;
				}
				x2 = minX;
				IL_19D:
				if (x2 <= maxX)
				{
					this.$current = new IntVec3(x2, thingCent.y, maxZ + 1);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_1AF:
				if (dir != LinkDirections.Left)
				{
					goto IL_227;
				}
				z = minZ;
				IL_215:
				if (z <= maxZ)
				{
					this.$current = new IntVec3(minX - 1, thingCent.y, z);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_227:
				if (dir != LinkDirections.Right)
				{
					goto IL_29F;
				}
				z2 = minZ;
				IL_28D:
				if (z2 <= maxZ)
				{
					this.$current = new IntVec3(maxX + 1, thingCent.y, z2);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_29F:
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsAdjacentAlongEdge>c__Iterator5 <CellsAdjacentAlongEdge>c__Iterator = new GenAdj.<CellsAdjacentAlongEdge>c__Iterator5();
				<CellsAdjacentAlongEdge>c__Iterator.thingCent = thingCent;
				<CellsAdjacentAlongEdge>c__Iterator.thingSize = thingSize;
				<CellsAdjacentAlongEdge>c__Iterator.thingRot = thingRot;
				<CellsAdjacentAlongEdge>c__Iterator.dir = dir;
				return <CellsAdjacentAlongEdge>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CellsAdjacent8WayAndInside>c__Iterator6 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Thing thing;

			internal IntVec3 <center>__0;

			internal IntVec2 <size>__0;

			internal Rot4 <rotation>__0;

			internal int <minX>__0;

			internal int <minZ>__0;

			internal int <maxX>__0;

			internal int <maxZ>__0;

			internal int <i>__1;

			internal int <j>__2;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CellsAdjacent8WayAndInside>c__Iterator6()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					center = thing.Position;
					size = thing.def.size;
					rotation = thing.Rotation;
					GenAdj.AdjustForRotation(ref center, ref size, rotation);
					minX = center.x - (size.x - 1) / 2 - 1;
					minZ = center.z - (size.z - 1) / 2 - 1;
					maxX = minX + size.x + 1;
					maxZ = minZ + size.z + 1;
					i = minX;
					goto IL_16A;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_14A:
				if (j <= maxZ)
				{
					this.$current = new IntVec3(i, 0, j);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_16A:
				if (i <= maxX)
				{
					j = minZ;
					goto IL_14A;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenAdj.<CellsAdjacent8WayAndInside>c__Iterator6 <CellsAdjacent8WayAndInside>c__Iterator = new GenAdj.<CellsAdjacent8WayAndInside>c__Iterator6();
				<CellsAdjacent8WayAndInside>c__Iterator.thing = thing;
				return <CellsAdjacent8WayAndInside>c__Iterator;
			}
		}
	}
}
