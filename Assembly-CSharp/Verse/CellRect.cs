using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public struct CellRect : IEquatable<CellRect>
	{
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			private CellRect ir;

			private int x;

			private int z;

			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			public bool MoveNext()
			{
				this.x++;
				if (this.x > this.ir.maxX)
				{
					this.x = this.ir.minX;
					this.z++;
				}
				if (this.z > this.ir.maxZ)
				{
					return false;
				}
				return true;
			}

			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			void IDisposable.Dispose()
			{
			}
		}

		public struct CellRectIterator
		{
			private int maxX;

			private int minX;

			private int maxZ;

			private int x;

			private int z;

			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			public CellRectIterator(CellRect cr)
			{
				this.minX = cr.minX;
				this.maxX = cr.maxX;
				this.maxZ = cr.maxZ;
				this.x = cr.minX;
				this.z = cr.minZ;
			}

			public void MoveNext()
			{
				this.x++;
				if (this.x > this.maxX)
				{
					this.x = this.minX;
					this.z++;
				}
			}

			public bool Done()
			{
				return this.z > this.maxZ;
			}
		}

		public int minX;

		public int maxX;

		public int minZ;

		public int maxZ;

		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		public int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		public int Width
		{
			get
			{
				if (this.minX > this.maxX)
				{
					return 0;
				}
				return this.maxX - this.minX + 1;
			}
			set
			{
				this.maxX = this.minX + Mathf.Max(value, 0) - 1;
			}
		}

		public int Height
		{
			get
			{
				if (this.minZ > this.maxZ)
				{
					return 0;
				}
				return this.maxZ - this.minZ + 1;
			}
			set
			{
				this.maxZ = this.minZ + Mathf.Max(value, 0) - 1;
			}
		}

		public IEnumerable<IntVec3> Corners
		{
			get
			{
				if (!this.IsEmpty)
				{
					yield return new IntVec3(this.minX, 0, this.minZ);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public IntVec3 BottomLeft
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
		}

		public IntVec3 TopRight
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
		}

		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)((float)this.minX + (float)this.Width / 2.0), 0f, (float)((float)this.minZ + (float)this.Height / 2.0));
			}
		}

		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)((float)this.maxX + 1.0)), 0f, Rand.Range((float)this.minZ, (float)((float)this.maxZ + 1.0)));
			}
		}

		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int z = this.minZ;
				int x;
				while (true)
				{
					if (z <= this.maxZ)
					{
						x = this.minX;
						if (x <= this.maxX)
							break;
						z++;
						continue;
					}
					yield break;
				}
				yield return new IntVec3(x, 0, z);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public IEnumerable<IntVec2> Cells2D
		{
			get
			{
				int z = this.minZ;
				int x;
				while (true)
				{
					if (z <= this.maxZ)
					{
						x = this.minX;
						if (x <= this.maxX)
							break;
						z++;
						continue;
					}
					yield break;
				}
				yield return new IntVec2(x, z);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public IEnumerable<IntVec3> EdgeCells
		{
			get
			{
				if (!this.IsEmpty)
				{
					int x4 = this.minX;
					int z4 = this.minZ;
					if (x4 <= this.maxX)
					{
						yield return new IntVec3(x4, 0, z4);
						/*Error: Unable to find new state assignment for yield return*/;
					}
					x4--;
					z4++;
					if (z4 <= this.maxZ)
					{
						yield return new IntVec3(x4, 0, z4);
						/*Error: Unable to find new state assignment for yield return*/;
					}
					z4--;
					x4--;
					if (x4 >= this.minX)
					{
						yield return new IntVec3(x4, 0, z4);
						/*Error: Unable to find new state assignment for yield return*/;
					}
					x4++;
					z4--;
					if (z4 > this.minZ)
					{
						yield return new IntVec3(x4, 0, z4);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
		}

		public int EdgeCellsCount
		{
			get
			{
				if (this.Area == 0)
				{
					return 0;
				}
				if (this.Area == 1)
				{
					return 1;
				}
				return this.Width * 2 + (this.Height - 2) * 2;
			}
		}

		public IEnumerable<IntVec3> AdjacentCellsCardinal
		{
			get
			{
				if (!this.IsEmpty)
				{
					int x = this.minX;
					if (x <= this.maxX)
					{
						yield return new IntVec3(x, 0, this.minZ - 1);
						/*Error: Unable to find new state assignment for yield return*/;
					}
					int z = this.minZ;
					if (z <= this.maxZ)
					{
						yield return new IntVec3(this.minX - 1, 0, z);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
		}

		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		public CellRectIterator GetIterator()
		{
			return new CellRectIterator(this);
		}

		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		public static CellRect WholeMap(Map map)
		{
			IntVec3 size = map.Size;
			int x = size.x;
			IntVec3 size2 = map.Size;
			return new CellRect(0, 0, x, size2.z);
		}

		public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
		{
			CellRect result = default(CellRect);
			result.minX = Mathf.Min(minX, maxX);
			result.minZ = Mathf.Min(minZ, maxZ);
			result.maxX = Mathf.Max(maxX, minX);
			result.maxZ = Mathf.Max(maxZ, minZ);
			return result;
		}

		public static CellRect FromLimits(IntVec3 first, IntVec3 second)
		{
			CellRect result = default(CellRect);
			result.minX = Mathf.Min(first.x, second.x);
			result.minZ = Mathf.Min(first.z, second.z);
			result.maxX = Mathf.Max(first.x, second.x);
			result.maxZ = Mathf.Max(first.z, second.z);
			return result;
		}

		public static CellRect CenteredOn(IntVec3 center, int radius)
		{
			CellRect result = default(CellRect);
			result.minX = center.x - radius;
			result.maxX = center.x + radius;
			result.minZ = center.z - radius;
			result.maxZ = center.z + radius;
			return result;
		}

		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect result = default(CellRect);
			result.minX = center.x - width / 2;
			result.minZ = center.z - height / 2;
			result.maxX = result.minX + width - 1;
			result.maxZ = result.minZ + height - 1;
			return result;
		}

		public static CellRect ViewRect(Map map)
		{
			if (Current.ProgramState == ProgramState.Playing && Find.VisibleMap == map && !WorldRendererUtility.WorldRenderedNow)
			{
				return Find.CameraDriver.CurrentViewRect;
			}
			return CellRect.Empty;
		}

		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		public bool InBounds(Map map)
		{
			int result;
			if (this.minX >= 0 && this.minZ >= 0)
			{
				int num = this.maxX;
				IntVec3 size = map.Size;
				if (num < size.x)
				{
					int num2 = this.maxZ;
					IntVec3 size2 = map.Size;
					result = ((num2 < size2.z) ? 1 : 0);
					goto IL_004a;
				}
			}
			result = 0;
			goto IL_004a;
			IL_004a:
			return (byte)result != 0;
		}

		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		public bool Overlaps(CellRect other)
		{
			if (!this.IsEmpty && !other.IsEmpty)
			{
				return this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ && this.minZ <= other.maxZ;
			}
			return false;
		}

		public bool IsOnEdge(IntVec3 c)
		{
			if (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ)
			{
				goto IL_00dd;
			}
			if (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ)
			{
				goto IL_00dd;
			}
			if (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX)
			{
				goto IL_00dd;
			}
			int result = (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX) ? 1 : 0;
			goto IL_00de;
			IL_00dd:
			result = 1;
			goto IL_00de;
			IL_00de:
			return (byte)result != 0;
		}

		public bool IsCorner(IntVec3 c)
		{
			if (c.x == this.minX && c.z == this.minZ)
			{
				goto IL_0092;
			}
			if (c.x == this.maxX && c.z == this.minZ)
			{
				goto IL_0092;
			}
			if (c.x == this.minX && c.z == this.maxZ)
			{
				goto IL_0092;
			}
			int result = (c.x == this.maxX && c.z == this.maxZ) ? 1 : 0;
			goto IL_0093;
			IL_0093:
			return (byte)result != 0;
			IL_0092:
			result = 1;
			goto IL_0093;
		}

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
			int num = this.maxX;
			IntVec3 size = map.Size;
			if (num > size.x - 1)
			{
				IntVec3 size2 = map.Size;
				this.maxX = size2.x - 1;
			}
			int num2 = this.maxZ;
			IntVec3 size3 = map.Size;
			if (num2 > size3.z - 1)
			{
				IntVec3 size4 = map.Size;
				this.maxZ = size4.z - 1;
			}
			return this;
		}

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

		public bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		public float ClosestDistSquaredTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return 0f;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((this.minX - c.x) * (this.minX - c.x));
			}
			if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((c.x - this.maxX) * (c.x - this.maxX));
			}
			if (c.z < this.minZ)
			{
				return (float)((this.minZ - c.z) * (this.minZ - c.z));
			}
			return (float)((c.z - this.maxZ) * (c.z - this.maxZ));
		}

		public IntVec3 ClosestCellTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return c;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.minX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.minX, 0, this.maxZ);
				}
				return new IntVec3(this.minX, 0, c.z);
			}
			if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.maxX, 0, this.maxZ);
				}
				return new IntVec3(this.maxX, 0, c.z);
			}
			if (c.z < this.minZ)
			{
				return new IntVec3(c.x, 0, this.minZ);
			}
			return new IntVec3(c.x, 0, this.maxZ);
		}

		public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				int x2 = this.minX;
				if (x2 <= this.maxX)
				{
					yield return new IntVec3(x2, 0, this.maxZ);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			else if (dir == Rot4.South)
			{
				int x = this.minX;
				if (x <= this.maxX)
				{
					yield return new IntVec3(x, 0, this.minZ);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			else if (dir == Rot4.West)
			{
				int z2 = this.minZ;
				if (z2 <= this.maxZ)
				{
					yield return new IntVec3(this.minX, 0, z2);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			else if (dir == Rot4.East)
			{
				int z = this.minZ;
				if (z <= this.maxZ)
				{
					yield return new IntVec3(this.maxX, 0, z);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width >= size.x && this.Height >= size.z)
			{
				if (size.x > 0 && size.z > 0 && !this.IsEmpty)
				{
					CellRect cellRect = this;
					cellRect.maxX -= size.x - 1;
					cellRect.maxZ -= size.z - 1;
					IntVec3 intVec = default(IntVec3);
					if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
					{
						if (predicate == null)
						{
							return true;
						}
						CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
						return predicate(obj);
					}).TryRandomElement<IntVec3>(out intVec))
					{
						rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
						return true;
					}
					rect = CellRect.Empty;
					return false;
				}
				rect = CellRect.Empty;
				return false;
			}
			rect = CellRect.Empty;
			return false;
		}

		public bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width >= size.x && this.Height >= size.z)
			{
				if (size.x > 0 && size.z > 0 && !this.IsEmpty)
				{
					CellRect cellRect = this;
					cellRect.maxX -= size.x - 1;
					cellRect.maxZ -= size.z - 1;
					IntVec3 intVec = default(IntVec3);
					if (cellRect.Cells.Where(delegate(IntVec3 x)
					{
						if (predicate == null)
						{
							return true;
						}
						CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
						return predicate(obj);
					}).TryRandomElement<IntVec3>(out intVec))
					{
						rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
						return true;
					}
					rect = CellRect.Empty;
					return false;
				}
				rect = CellRect.Empty;
				return false;
			}
			rect = CellRect.Empty;
			return false;
		}

		public CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		public CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		public void DebugDraw()
		{
			float y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
			Vector3 vector = new Vector3((float)this.minX, y, (float)this.minZ);
			Vector3 vector2 = new Vector3((float)this.minX, y, (float)(this.maxZ + 1));
			Vector3 vector3 = new Vector3((float)(this.maxX + 1), y, (float)(this.maxZ + 1));
			Vector3 vector4 = new Vector3((float)(this.maxX + 1), y, (float)this.minZ);
			GenDraw.DrawLineBetween(vector, vector2);
			GenDraw.DrawLineBetween(vector2, vector3);
			GenDraw.DrawLineBetween(vector3, vector4);
			GenDraw.DrawLineBetween(vector4, vector);
		}

		public IEnumerator<IntVec3> GetEnumerator()
		{
			return (IEnumerator<IntVec3>)(object)new Enumerator(this);
		}

		public override string ToString()
		{
			return "(" + this.minX + "," + this.minZ + "," + this.maxX + "," + this.maxZ + ")";
		}

		public static CellRect FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',');
			int num = Convert.ToInt32(array[0]);
			int num2 = Convert.ToInt32(array[1]);
			int num3 = Convert.ToInt32(array[2]);
			int num4 = Convert.ToInt32(array[3]);
			return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.minX);
			seed = Gen.HashCombineInt(seed, this.maxX);
			seed = Gen.HashCombineInt(seed, this.minZ);
			return Gen.HashCombineInt(seed, this.maxZ);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is CellRect))
			{
				return false;
			}
			return this.Equals((CellRect)obj);
		}

		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}
	}
}
