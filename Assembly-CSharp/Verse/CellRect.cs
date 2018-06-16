using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE2 RID: 3810
	public struct CellRect : IEquatable<CellRect>
	{
		// Token: 0x06005A0F RID: 23055 RVA: 0x002E276B File Offset: 0x002E0B6B
		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06005A10 RID: 23056 RVA: 0x002E2794 File Offset: 0x002E0B94
		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06005A11 RID: 23057 RVA: 0x002E27B4 File Offset: 0x002E0BB4
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06005A12 RID: 23058 RVA: 0x002E27E4 File Offset: 0x002E0BE4
		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06005A13 RID: 23059 RVA: 0x002E2808 File Offset: 0x002E0C08
		// (set) Token: 0x06005A14 RID: 23060 RVA: 0x002E2844 File Offset: 0x002E0C44
		public int Width
		{
			get
			{
				int result;
				if (this.minX > this.maxX)
				{
					result = 0;
				}
				else
				{
					result = this.maxX - this.minX + 1;
				}
				return result;
			}
			set
			{
				this.maxX = this.minX + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06005A15 RID: 23061 RVA: 0x002E2860 File Offset: 0x002E0C60
		// (set) Token: 0x06005A16 RID: 23062 RVA: 0x002E289C File Offset: 0x002E0C9C
		public int Height
		{
			get
			{
				int result;
				if (this.minZ > this.maxZ)
				{
					result = 0;
				}
				else
				{
					result = this.maxZ - this.minZ + 1;
				}
				return result;
			}
			set
			{
				this.maxZ = this.minZ + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06005A17 RID: 23063 RVA: 0x002E28B8 File Offset: 0x002E0CB8
		public IEnumerable<IntVec3> Corners
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				yield return new IntVec3(this.minX, 0, this.minZ);
				if (this.Width > 1)
				{
					yield return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (this.Height > 1)
				{
					yield return new IntVec3(this.minX, 0, this.maxZ);
					if (this.Width > 1)
					{
						yield return new IntVec3(this.maxX, 0, this.maxZ);
					}
				}
				yield break;
			}
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x002E28E8 File Offset: 0x002E0CE8
		public CellRect.CellRectIterator GetIterator()
		{
			return new CellRect.CellRectIterator(this);
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06005A19 RID: 23065 RVA: 0x002E2908 File Offset: 0x002E0D08
		public IntVec3 BottomLeft
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06005A1A RID: 23066 RVA: 0x002E2930 File Offset: 0x002E0D30
		public IntVec3 TopRight
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06005A1B RID: 23067 RVA: 0x002E2958 File Offset: 0x002E0D58
		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06005A1C RID: 23068 RVA: 0x002E2998 File Offset: 0x002E0D98
		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x002E29D4 File Offset: 0x002E0DD4
		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06005A1E RID: 23070 RVA: 0x002E2A20 File Offset: 0x002E0E20
		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06005A1F RID: 23071 RVA: 0x002E2A74 File Offset: 0x002E0E74
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				for (int z = this.minZ; z <= this.maxZ; z++)
				{
					for (int x = this.minX; x <= this.maxX; x++)
					{
						yield return new IntVec3(x, 0, z);
					}
				}
				yield break;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x002E2AA4 File Offset: 0x002E0EA4
		public IEnumerable<IntVec2> Cells2D
		{
			get
			{
				for (int z = this.minZ; z <= this.maxZ; z++)
				{
					for (int x = this.minX; x <= this.maxX; x++)
					{
						yield return new IntVec2(x, z);
					}
				}
				yield break;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06005A21 RID: 23073 RVA: 0x002E2AD4 File Offset: 0x002E0ED4
		public IEnumerable<IntVec3> EdgeCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int x = this.minX;
				int z = this.minZ;
				while (x <= this.maxX)
				{
					yield return new IntVec3(x, 0, z);
					x++;
				}
				x--;
				for (z++; z <= this.maxZ; z++)
				{
					yield return new IntVec3(x, 0, z);
				}
				z--;
				for (x--; x >= this.minX; x--)
				{
					yield return new IntVec3(x, 0, z);
				}
				x++;
				for (z--; z > this.minZ; z--)
				{
					yield return new IntVec3(x, 0, z);
				}
				yield break;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06005A22 RID: 23074 RVA: 0x002E2B04 File Offset: 0x002E0F04
		public int EdgeCellsCount
		{
			get
			{
				int result;
				if (this.Area == 0)
				{
					result = 0;
				}
				else if (this.Area == 1)
				{
					result = 1;
				}
				else
				{
					result = this.Width * 2 + (this.Height - 2) * 2;
				}
				return result;
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x002E2B54 File Offset: 0x002E0F54
		public IEnumerable<IntVec3> AdjacentCellsCardinal
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				for (int x = this.minX; x <= this.maxX; x++)
				{
					yield return new IntVec3(x, 0, this.minZ - 1);
					yield return new IntVec3(x, 0, this.maxZ + 1);
				}
				for (int z = this.minZ; z <= this.maxZ; z++)
				{
					yield return new IntVec3(this.minX - 1, 0, z);
					yield return new IntVec3(this.maxX + 1, 0, z);
				}
				yield break;
			}
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x002E2B84 File Offset: 0x002E0F84
		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x002E2BA4 File Offset: 0x002E0FA4
		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x002E2BC4 File Offset: 0x002E0FC4
		public static CellRect WholeMap(Map map)
		{
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x002E2BFC File Offset: 0x002E0FFC
		public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
		{
			return new CellRect
			{
				minX = Mathf.Min(minX, maxX),
				minZ = Mathf.Min(minZ, maxZ),
				maxX = Mathf.Max(maxX, minX),
				maxZ = Mathf.Max(maxZ, minZ)
			};
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x002E2C54 File Offset: 0x002E1054
		public static CellRect FromLimits(IntVec3 first, IntVec3 second)
		{
			return new CellRect
			{
				minX = Mathf.Min(first.x, second.x),
				minZ = Mathf.Min(first.z, second.z),
				maxX = Mathf.Max(first.x, second.x),
				maxZ = Mathf.Max(first.z, second.z)
			};
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x002E2CDC File Offset: 0x002E10DC
		public static CellRect CenteredOn(IntVec3 center, int radius)
		{
			return new CellRect
			{
				minX = center.x - radius,
				maxX = center.x + radius,
				minZ = center.z - radius,
				maxZ = center.z + radius
			};
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x002E2D3C File Offset: 0x002E113C
		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect result = default(CellRect);
			result.minX = center.x - width / 2;
			result.minZ = center.z - height / 2;
			result.maxX = result.minX + width - 1;
			result.maxZ = result.minZ + height - 1;
			return result;
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x002E2DA4 File Offset: 0x002E11A4
		public static CellRect ViewRect(Map map)
		{
			CellRect result;
			if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldRenderedNow)
			{
				result = CellRect.Empty;
			}
			else
			{
				result = Find.CameraDriver.CurrentViewRect;
			}
			return result;
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x002E2DF0 File Offset: 0x002E11F0
		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x002E2E1C File Offset: 0x002E121C
		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x002E2E7C File Offset: 0x002E127C
		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x002E2EB0 File Offset: 0x002E12B0
		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x002E2F30 File Offset: 0x002E1330
		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x002E3024 File Offset: 0x002E1424
		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x002E30A4 File Offset: 0x002E14A4
		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x002E314C File Offset: 0x002E154C
		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x002E31D4 File Offset: 0x002E15D4
		public CellRect ClipInsideMap(Map map)
		{
			if (this.minX < 0)
			{
				this.minX = 0;
			}
			if (this.minZ < 0)
			{
				this.minZ = 0;
			}
			if (this.maxX > map.Size.x - 1)
			{
				this.maxX = map.Size.x - 1;
			}
			if (this.maxZ > map.Size.z - 1)
			{
				this.maxZ = map.Size.z - 1;
			}
			return this;
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x002E327C File Offset: 0x002E167C
		public CellRect ClipInsideRect(CellRect otherRect)
		{
			if (this.minX < otherRect.minX)
			{
				this.minX = otherRect.minX;
			}
			if (this.maxX > otherRect.maxX)
			{
				this.maxX = otherRect.maxX;
			}
			if (this.minZ < otherRect.minZ)
			{
				this.minZ = otherRect.minZ;
			}
			if (this.maxZ > otherRect.maxZ)
			{
				this.maxZ = otherRect.maxZ;
			}
			return this;
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x002E3314 File Offset: 0x002E1714
		public bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x002E3374 File Offset: 0x002E1774
		public float ClosestDistSquaredTo(IntVec3 c)
		{
			float result;
			if (this.Contains(c))
			{
				result = 0f;
			}
			else if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					result = (float)(c - new IntVec3(this.minX, 0, this.minZ)).LengthHorizontalSquared;
				}
				else if (c.z > this.maxZ)
				{
					result = (float)(c - new IntVec3(this.minX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				else
				{
					result = (float)((this.minX - c.x) * (this.minX - c.x));
				}
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					result = (float)(c - new IntVec3(this.maxX, 0, this.minZ)).LengthHorizontalSquared;
				}
				else if (c.z > this.maxZ)
				{
					result = (float)(c - new IntVec3(this.maxX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				else
				{
					result = (float)((c.x - this.maxX) * (c.x - this.maxX));
				}
			}
			else if (c.z < this.minZ)
			{
				result = (float)((this.minZ - c.z) * (this.minZ - c.z));
			}
			else
			{
				result = (float)((c.z - this.maxZ) * (c.z - this.maxZ));
			}
			return result;
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x002E3548 File Offset: 0x002E1948
		public IntVec3 ClosestCellTo(IntVec3 c)
		{
			IntVec3 result;
			if (this.Contains(c))
			{
				result = c;
			}
			else if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					result = new IntVec3(this.minX, 0, this.minZ);
				}
				else if (c.z > this.maxZ)
				{
					result = new IntVec3(this.minX, 0, this.maxZ);
				}
				else
				{
					result = new IntVec3(this.minX, 0, c.z);
				}
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					result = new IntVec3(this.maxX, 0, this.minZ);
				}
				else if (c.z > this.maxZ)
				{
					result = new IntVec3(this.maxX, 0, this.maxZ);
				}
				else
				{
					result = new IntVec3(this.maxX, 0, c.z);
				}
			}
			else if (c.z < this.minZ)
			{
				result = new IntVec3(c.x, 0, this.minZ);
			}
			else
			{
				result = new IntVec3(c.x, 0, this.maxZ);
			}
			return result;
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x002E36B0 File Offset: 0x002E1AB0
		public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				for (int x = this.minX; x <= this.maxX; x++)
				{
					yield return new IntVec3(x, 0, this.maxZ);
				}
			}
			else if (dir == Rot4.South)
			{
				for (int x2 = this.minX; x2 <= this.maxX; x2++)
				{
					yield return new IntVec3(x2, 0, this.minZ);
				}
			}
			else if (dir == Rot4.West)
			{
				for (int z = this.minZ; z <= this.maxZ; z++)
				{
					yield return new IntVec3(this.minX, 0, z);
				}
			}
			else if (dir == Rot4.East)
			{
				for (int z2 = this.minZ; z2 <= this.maxZ; z2++)
				{
					yield return new IntVec3(this.maxX, 0, z2);
				}
			}
			yield break;
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x002E36E8 File Offset: 0x002E1AE8
		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			bool result;
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				result = false;
			}
			else if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				result = false;
			}
			else
			{
				CellRect cellRect = this;
				cellRect.maxX -= size.x - 1;
				cellRect.maxZ -= size.z - 1;
				IntVec3 intVec;
				if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
				{
					bool result2;
					if (predicate == null)
					{
						result2 = true;
					}
					else
					{
						CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
						result2 = predicate(obj);
					}
					return result2;
				}).TryRandomElement(out intVec))
				{
					rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
					result = true;
				}
				else
				{
					rect = CellRect.Empty;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x002E3830 File Offset: 0x002E1C30
		public bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			bool result;
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				result = false;
			}
			else if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				result = false;
			}
			else
			{
				CellRect cellRect = this;
				cellRect.maxX -= size.x - 1;
				cellRect.maxZ -= size.z - 1;
				IntVec3 intVec;
				if (cellRect.Cells.Where(delegate(IntVec3 x)
				{
					bool result2;
					if (predicate == null)
					{
						result2 = true;
					}
					else
					{
						CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
						result2 = predicate(obj);
					}
					return result2;
				}).TryRandomElement(out intVec))
				{
					rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
					result = true;
				}
				else
				{
					rect = CellRect.Empty;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x002E3978 File Offset: 0x002E1D78
		public CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x002E39D4 File Offset: 0x002E1DD4
		public CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x002E39F4 File Offset: 0x002E1DF4
		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		// Token: 0x06005A3F RID: 23103 RVA: 0x002E3A30 File Offset: 0x002E1E30
		public void DebugDraw()
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 vector = new Vector3((float)this.minX, y, (float)this.minZ);
			Vector3 vector2 = new Vector3((float)this.minX, y, (float)(this.maxZ + 1));
			Vector3 vector3 = new Vector3((float)(this.maxX + 1), y, (float)(this.maxZ + 1));
			Vector3 vector4 = new Vector3((float)(this.maxX + 1), y, (float)this.minZ);
			GenDraw.DrawLineBetween(vector, vector2);
			GenDraw.DrawLineBetween(vector2, vector3);
			GenDraw.DrawLineBetween(vector3, vector4);
			GenDraw.DrawLineBetween(vector4, vector);
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x002E3AC4 File Offset: 0x002E1EC4
		public IEnumerator<IntVec3> GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x002E3AEC File Offset: 0x002E1EEC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.minX,
				",",
				this.minZ,
				",",
				this.maxX,
				",",
				this.maxZ,
				")"
			});
		}

		// Token: 0x06005A42 RID: 23106 RVA: 0x002E3B70 File Offset: 0x002E1F70
		public static CellRect FromString(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			int num = Convert.ToInt32(array[0]);
			int num2 = Convert.ToInt32(array[1]);
			int num3 = Convert.ToInt32(array[2]);
			int num4 = Convert.ToInt32(array[3]);
			return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		// Token: 0x06005A43 RID: 23107 RVA: 0x002E3BF8 File Offset: 0x002E1FF8
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.minX);
			seed = Gen.HashCombineInt(seed, this.maxX);
			seed = Gen.HashCombineInt(seed, this.minZ);
			return Gen.HashCombineInt(seed, this.maxZ);
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x002E3C44 File Offset: 0x002E2044
		public override bool Equals(object obj)
		{
			return obj is CellRect && this.Equals((CellRect)obj);
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x002E3C78 File Offset: 0x002E2078
		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		// Token: 0x04003C67 RID: 15463
		public int minX;

		// Token: 0x04003C68 RID: 15464
		public int maxX;

		// Token: 0x04003C69 RID: 15465
		public int minZ;

		// Token: 0x04003C6A RID: 15466
		public int maxZ;

		// Token: 0x02000EE3 RID: 3811
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			// Token: 0x06005A46 RID: 23110 RVA: 0x002E3CD5 File Offset: 0x002E20D5
			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			// Token: 0x17000E3E RID: 3646
			// (get) Token: 0x06005A47 RID: 23111 RVA: 0x002E3CFC File Offset: 0x002E20FC
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x17000E3D RID: 3645
			// (get) Token: 0x06005A48 RID: 23112 RVA: 0x002E3D24 File Offset: 0x002E2124
			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x06005A49 RID: 23113 RVA: 0x002E3D50 File Offset: 0x002E2150
			public bool MoveNext()
			{
				this.x++;
				if (this.x > this.ir.maxX)
				{
					this.x = this.ir.minX;
					this.z++;
				}
				return this.z <= this.ir.maxZ;
			}

			// Token: 0x06005A4A RID: 23114 RVA: 0x002E3DC8 File Offset: 0x002E21C8
			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			// Token: 0x06005A4B RID: 23115 RVA: 0x002E3DEF File Offset: 0x002E21EF
			void IDisposable.Dispose()
			{
			}

			// Token: 0x04003C6B RID: 15467
			private CellRect ir;

			// Token: 0x04003C6C RID: 15468
			private int x;

			// Token: 0x04003C6D RID: 15469
			private int z;
		}

		// Token: 0x02000EE4 RID: 3812
		public struct CellRectIterator
		{
			// Token: 0x06005A4C RID: 23116 RVA: 0x002E3DF4 File Offset: 0x002E21F4
			public CellRectIterator(CellRect cr)
			{
				this.minX = cr.minX;
				this.maxX = cr.maxX;
				this.maxZ = cr.maxZ;
				this.x = cr.minX;
				this.z = cr.minZ;
			}

			// Token: 0x17000E3F RID: 3647
			// (get) Token: 0x06005A4D RID: 23117 RVA: 0x002E3E44 File Offset: 0x002E2244
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x06005A4E RID: 23118 RVA: 0x002E3E6B File Offset: 0x002E226B
			public void MoveNext()
			{
				this.x++;
				if (this.x > this.maxX)
				{
					this.x = this.minX;
					this.z++;
				}
			}

			// Token: 0x06005A4F RID: 23119 RVA: 0x002E3EAC File Offset: 0x002E22AC
			public bool Done()
			{
				return this.z > this.maxZ;
			}

			// Token: 0x04003C6E RID: 15470
			private int maxX;

			// Token: 0x04003C6F RID: 15471
			private int minX;

			// Token: 0x04003C70 RID: 15472
			private int maxZ;

			// Token: 0x04003C71 RID: 15473
			private int x;

			// Token: 0x04003C72 RID: 15474
			private int z;
		}
	}
}
