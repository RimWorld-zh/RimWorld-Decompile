using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EE0 RID: 3808
	public struct CellRect : IEquatable<CellRect>
	{
		// Token: 0x06005A2E RID: 23086 RVA: 0x002E4657 File Offset: 0x002E2A57
		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06005A2F RID: 23087 RVA: 0x002E4680 File Offset: 0x002E2A80
		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06005A30 RID: 23088 RVA: 0x002E46A0 File Offset: 0x002E2AA0
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x06005A31 RID: 23089 RVA: 0x002E46D0 File Offset: 0x002E2AD0
		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x06005A32 RID: 23090 RVA: 0x002E46F4 File Offset: 0x002E2AF4
		// (set) Token: 0x06005A33 RID: 23091 RVA: 0x002E4730 File Offset: 0x002E2B30
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

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x06005A34 RID: 23092 RVA: 0x002E474C File Offset: 0x002E2B4C
		// (set) Token: 0x06005A35 RID: 23093 RVA: 0x002E4788 File Offset: 0x002E2B88
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

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06005A36 RID: 23094 RVA: 0x002E47A4 File Offset: 0x002E2BA4
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

		// Token: 0x06005A37 RID: 23095 RVA: 0x002E47D4 File Offset: 0x002E2BD4
		public CellRect.CellRectIterator GetIterator()
		{
			return new CellRect.CellRectIterator(this);
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06005A38 RID: 23096 RVA: 0x002E47F4 File Offset: 0x002E2BF4
		public IntVec3 BottomLeft
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06005A39 RID: 23097 RVA: 0x002E481C File Offset: 0x002E2C1C
		public IntVec3 TopRight
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06005A3A RID: 23098 RVA: 0x002E4844 File Offset: 0x002E2C44
		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x06005A3B RID: 23099 RVA: 0x002E4884 File Offset: 0x002E2C84
		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x06005A3C RID: 23100 RVA: 0x002E48C0 File Offset: 0x002E2CC0
		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x06005A3D RID: 23101 RVA: 0x002E490C File Offset: 0x002E2D0C
		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06005A3E RID: 23102 RVA: 0x002E4960 File Offset: 0x002E2D60
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

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06005A3F RID: 23103 RVA: 0x002E4990 File Offset: 0x002E2D90
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

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06005A40 RID: 23104 RVA: 0x002E49C0 File Offset: 0x002E2DC0
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

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06005A41 RID: 23105 RVA: 0x002E49F0 File Offset: 0x002E2DF0
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

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06005A42 RID: 23106 RVA: 0x002E4A40 File Offset: 0x002E2E40
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

		// Token: 0x06005A43 RID: 23107 RVA: 0x002E4A70 File Offset: 0x002E2E70
		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06005A44 RID: 23108 RVA: 0x002E4A90 File Offset: 0x002E2E90
		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06005A45 RID: 23109 RVA: 0x002E4AB0 File Offset: 0x002E2EB0
		public static CellRect WholeMap(Map map)
		{
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

		// Token: 0x06005A46 RID: 23110 RVA: 0x002E4AE8 File Offset: 0x002E2EE8
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

		// Token: 0x06005A47 RID: 23111 RVA: 0x002E4B40 File Offset: 0x002E2F40
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

		// Token: 0x06005A48 RID: 23112 RVA: 0x002E4BC8 File Offset: 0x002E2FC8
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

		// Token: 0x06005A49 RID: 23113 RVA: 0x002E4C28 File Offset: 0x002E3028
		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect result = default(CellRect);
			result.minX = center.x - width / 2;
			result.minZ = center.z - height / 2;
			result.maxX = result.minX + width - 1;
			result.maxZ = result.minZ + height - 1;
			return result;
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x002E4C90 File Offset: 0x002E3090
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

		// Token: 0x06005A4B RID: 23115 RVA: 0x002E4CDC File Offset: 0x002E30DC
		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		// Token: 0x06005A4C RID: 23116 RVA: 0x002E4D08 File Offset: 0x002E3108
		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x002E4D68 File Offset: 0x002E3168
		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x002E4D9C File Offset: 0x002E319C
		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x002E4E1C File Offset: 0x002E321C
		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x002E4F10 File Offset: 0x002E3310
		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x002E4F90 File Offset: 0x002E3390
		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x002E5038 File Offset: 0x002E3438
		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x002E50C0 File Offset: 0x002E34C0
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

		// Token: 0x06005A54 RID: 23124 RVA: 0x002E5168 File Offset: 0x002E3568
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

		// Token: 0x06005A55 RID: 23125 RVA: 0x002E5200 File Offset: 0x002E3600
		public bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x002E5260 File Offset: 0x002E3660
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

		// Token: 0x06005A57 RID: 23127 RVA: 0x002E5434 File Offset: 0x002E3834
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

		// Token: 0x06005A58 RID: 23128 RVA: 0x002E559C File Offset: 0x002E399C
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

		// Token: 0x06005A59 RID: 23129 RVA: 0x002E55D4 File Offset: 0x002E39D4
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

		// Token: 0x06005A5A RID: 23130 RVA: 0x002E571C File Offset: 0x002E3B1C
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

		// Token: 0x06005A5B RID: 23131 RVA: 0x002E5864 File Offset: 0x002E3C64
		public CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x06005A5C RID: 23132 RVA: 0x002E58C0 File Offset: 0x002E3CC0
		public CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x06005A5D RID: 23133 RVA: 0x002E58E0 File Offset: 0x002E3CE0
		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		// Token: 0x06005A5E RID: 23134 RVA: 0x002E591C File Offset: 0x002E3D1C
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

		// Token: 0x06005A5F RID: 23135 RVA: 0x002E59B0 File Offset: 0x002E3DB0
		public IEnumerator<IntVec3> GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

		// Token: 0x06005A60 RID: 23136 RVA: 0x002E59D8 File Offset: 0x002E3DD8
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

		// Token: 0x06005A61 RID: 23137 RVA: 0x002E5A5C File Offset: 0x002E3E5C
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

		// Token: 0x06005A62 RID: 23138 RVA: 0x002E5AE4 File Offset: 0x002E3EE4
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.minX);
			seed = Gen.HashCombineInt(seed, this.maxX);
			seed = Gen.HashCombineInt(seed, this.minZ);
			return Gen.HashCombineInt(seed, this.maxZ);
		}

		// Token: 0x06005A63 RID: 23139 RVA: 0x002E5B30 File Offset: 0x002E3F30
		public override bool Equals(object obj)
		{
			return obj is CellRect && this.Equals((CellRect)obj);
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x002E5B64 File Offset: 0x002E3F64
		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		// Token: 0x04003C76 RID: 15478
		public int minX;

		// Token: 0x04003C77 RID: 15479
		public int maxX;

		// Token: 0x04003C78 RID: 15480
		public int minZ;

		// Token: 0x04003C79 RID: 15481
		public int maxZ;

		// Token: 0x02000EE1 RID: 3809
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			// Token: 0x06005A65 RID: 23141 RVA: 0x002E5BC1 File Offset: 0x002E3FC1
			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			// Token: 0x17000E40 RID: 3648
			// (get) Token: 0x06005A66 RID: 23142 RVA: 0x002E5BE8 File Offset: 0x002E3FE8
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x17000E3F RID: 3647
			// (get) Token: 0x06005A67 RID: 23143 RVA: 0x002E5C10 File Offset: 0x002E4010
			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x06005A68 RID: 23144 RVA: 0x002E5C3C File Offset: 0x002E403C
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

			// Token: 0x06005A69 RID: 23145 RVA: 0x002E5CB4 File Offset: 0x002E40B4
			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			// Token: 0x06005A6A RID: 23146 RVA: 0x002E5CDB File Offset: 0x002E40DB
			void IDisposable.Dispose()
			{
			}

			// Token: 0x04003C7A RID: 15482
			private CellRect ir;

			// Token: 0x04003C7B RID: 15483
			private int x;

			// Token: 0x04003C7C RID: 15484
			private int z;
		}

		// Token: 0x02000EE2 RID: 3810
		public struct CellRectIterator
		{
			// Token: 0x06005A6B RID: 23147 RVA: 0x002E5CE0 File Offset: 0x002E40E0
			public CellRectIterator(CellRect cr)
			{
				this.minX = cr.minX;
				this.maxX = cr.maxX;
				this.maxZ = cr.maxZ;
				this.x = cr.minX;
				this.z = cr.minZ;
			}

			// Token: 0x17000E41 RID: 3649
			// (get) Token: 0x06005A6C RID: 23148 RVA: 0x002E5D30 File Offset: 0x002E4130
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x06005A6D RID: 23149 RVA: 0x002E5D57 File Offset: 0x002E4157
			public void MoveNext()
			{
				this.x++;
				if (this.x > this.maxX)
				{
					this.x = this.minX;
					this.z++;
				}
			}

			// Token: 0x06005A6E RID: 23150 RVA: 0x002E5D98 File Offset: 0x002E4198
			public bool Done()
			{
				return this.z > this.maxZ;
			}

			// Token: 0x04003C7D RID: 15485
			private int maxX;

			// Token: 0x04003C7E RID: 15486
			private int minX;

			// Token: 0x04003C7F RID: 15487
			private int maxZ;

			// Token: 0x04003C80 RID: 15488
			private int x;

			// Token: 0x04003C81 RID: 15489
			private int z;
		}
	}
}
