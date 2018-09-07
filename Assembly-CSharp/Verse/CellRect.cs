using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public struct CellRect : IEquatable<CellRect>
	{
		public int minX;

		public int maxX;

		public int minZ;

		public int maxZ;

		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

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

		public CellRect.CellRectIterator GetIterator()
		{
			return new CellRect.CellRectIterator(this);
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
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

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
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

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
			if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldRenderedNow)
			{
				return CellRect.Empty;
			}
			return Find.CameraDriver.CurrentViewRect;
		}

		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
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
			else if (c.x > this.maxX)
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
			else
			{
				if (c.z < this.minZ)
				{
					return (float)((this.minZ - c.z) * (this.minZ - c.z));
				}
				return (float)((c.z - this.maxZ) * (c.z - this.maxZ));
			}
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
			else if (c.x > this.maxX)
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
			else
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(c.x, 0, this.minZ);
				}
				return new IntVec3(c.x, 0, this.maxZ);
			}
		}

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

		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		public bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Cells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
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

		public IEnumerator<IntVec3> GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

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
			return obj is CellRect && this.Equals((CellRect)obj);
		}

		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			private CellRect ir;

			private int x;

			private int z;

			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

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

			public CellRectIterator(CellRect cr)
			{
				this.minX = cr.minX;
				this.maxX = cr.maxX;
				this.maxZ = cr.maxZ;
				this.x = cr.minX;
				this.z = cr.minZ;
			}

			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal CellRect $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (base.IsEmpty)
					{
						return false;
					}
					this.$current = new IntVec3(this.minX, 0, this.minZ);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					if (base.Width > 1)
					{
						this.$current = new IntVec3(this.maxX, 0, this.minZ);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2u:
					break;
				case 3u:
					if (base.Width > 1)
					{
						this.$current = new IntVec3(this.maxX, 0, this.maxZ);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					goto IL_14D;
				case 4u:
					goto IL_14D;
				default:
					return false;
				}
				if (base.Height > 1)
				{
					this.$current = new IntVec3(this.minX, 0, this.maxZ);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_14D:
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
				CellRect.<>c__Iterator0 <>c__Iterator = new CellRect.<>c__Iterator0();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <z>__1;

			internal int <x>__2;

			internal CellRect $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					z = this.minZ;
					goto IL_AB;
				case 1u:
					x++;
					break;
				default:
					return false;
				}
				IL_87:
				if (x <= this.maxX)
				{
					this.$current = new IntVec3(x, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				z++;
				IL_AB:
				if (z <= this.maxZ)
				{
					x = this.minX;
					goto IL_87;
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
				CellRect.<>c__Iterator1 <>c__Iterator = new CellRect.<>c__Iterator1();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<IntVec2>, IEnumerator, IDisposable, IEnumerator<IntVec2>
		{
			internal int <z>__1;

			internal int <x>__2;

			internal CellRect $this;

			internal IntVec2 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					z = this.minZ;
					goto IL_AA;
				case 1u:
					x++;
					break;
				default:
					return false;
				}
				IL_86:
				if (x <= this.maxX)
				{
					this.$current = new IntVec2(x, z);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				z++;
				IL_AA:
				if (z <= this.maxZ)
				{
					x = this.minX;
					goto IL_86;
				}
				this.$PC = -1;
				return false;
			}

			IntVec2 IEnumerator<IntVec2>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec2>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec2> IEnumerable<IntVec2>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CellRect.<>c__Iterator2 <>c__Iterator = new CellRect.<>c__Iterator2();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator3 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <x>__0;

			internal int <z>__0;

			internal CellRect $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (base.IsEmpty)
					{
						return false;
					}
					x = this.minX;
					z = this.minZ;
					break;
				case 1u:
					x++;
					break;
				case 2u:
					z++;
					goto IL_114;
				case 3u:
					x--;
					goto IL_185;
				case 4u:
					z--;
					goto IL_1F6;
				default:
					return false;
				}
				if (x <= this.maxX)
				{
					this.$current = new IntVec3(x, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				x--;
				z++;
				IL_114:
				if (z <= this.maxZ)
				{
					this.$current = new IntVec3(x, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				z--;
				x--;
				IL_185:
				if (x >= this.minX)
				{
					this.$current = new IntVec3(x, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				x++;
				z--;
				IL_1F6:
				if (z > this.minZ)
				{
					this.$current = new IntVec3(x, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
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
				CellRect.<>c__Iterator3 <>c__Iterator = new CellRect.<>c__Iterator3();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator4 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <x>__1;

			internal int <z>__2;

			internal CellRect $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator4()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (base.IsEmpty)
					{
						return false;
					}
					x = this.minX;
					break;
				case 1u:
					this.$current = new IntVec3(x, 0, this.maxZ + 1);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					x++;
					break;
				case 3u:
					this.$current = new IntVec3(this.maxX + 1, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					z++;
					goto IL_16C;
				default:
					return false;
				}
				if (x <= this.maxX)
				{
					this.$current = new IntVec3(x, 0, this.minZ - 1);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				z = this.minZ;
				IL_16C:
				if (z <= this.maxZ)
				{
					this.$current = new IntVec3(this.minX - 1, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
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
				CellRect.<>c__Iterator4 <>c__Iterator = new CellRect.<>c__Iterator4();
				<>c__Iterator.$this = ref this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetEdgeCells>c__Iterator5 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Rot4 dir;

			internal int <x>__1;

			internal int <x>__2;

			internal int <z>__3;

			internal int <z>__4;

			internal CellRect $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetEdgeCells>c__Iterator5()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (dir == Rot4.North)
					{
						x = this.minX;
					}
					else
					{
						if (dir == Rot4.South)
						{
							x2 = this.minX;
							goto IL_11C;
						}
						if (dir == Rot4.West)
						{
							z = this.minZ;
							goto IL_1A1;
						}
						if (dir == Rot4.East)
						{
							z2 = this.minZ;
							goto IL_226;
						}
						goto IL_23C;
					}
					break;
				case 1u:
					x++;
					break;
				case 2u:
					x2++;
					goto IL_11C;
				case 3u:
					z++;
					goto IL_1A1;
				case 4u:
					z2++;
					goto IL_226;
				default:
					return false;
				}
				if (x > this.maxX)
				{
					goto IL_23C;
				}
				this.$current = new IntVec3(x, 0, this.maxZ);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				IL_11C:
				if (x2 > this.maxX)
				{
					goto IL_23C;
				}
				this.$current = new IntVec3(x2, 0, this.minZ);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_1A1:
				if (z > this.maxZ)
				{
					goto IL_23C;
				}
				this.$current = new IntVec3(this.minX, 0, z);
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
				IL_226:
				if (z2 <= this.maxZ)
				{
					this.$current = new IntVec3(this.maxX, 0, z2);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_23C:
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
				CellRect.<GetEdgeCells>c__Iterator5 <GetEdgeCells>c__Iterator = new CellRect.<GetEdgeCells>c__Iterator5();
				<GetEdgeCells>c__Iterator.$this = ref this;
				<GetEdgeCells>c__Iterator.dir = dir;
				return <GetEdgeCells>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomInnerRectTouchingEdge>c__AnonStorey6
		{
			internal Predicate<CellRect> predicate;

			internal IntVec2 size;

			public <TryFindRandomInnerRectTouchingEdge>c__AnonStorey6()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				if (this.predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, this.size.x, this.size.z);
				return this.predicate(obj);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomInnerRect>c__AnonStorey7
		{
			internal Predicate<CellRect> predicate;

			internal IntVec2 size;

			public <TryFindRandomInnerRect>c__AnonStorey7()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				if (this.predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, this.size.x, this.size.z);
				return this.predicate(obj);
			}
		}
	}
}
