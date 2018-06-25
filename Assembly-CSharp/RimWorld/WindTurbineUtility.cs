using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
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
				rectB.minZ = (rectA.minZ = center.z - 3);
				rectB.maxZ = (rectA.maxZ = center.z + 3);
			}
			else
			{
				rectA.minZ = center.z + 2 + offset;
				rectA.maxZ = center.z + 2 + neDist + offset;
				rectB.minZ = center.z - 1 - swDist + offset;
				rectB.maxZ = center.z - 1 + offset;
				rectB.minX = (rectA.minX = center.x - 3);
				rectB.maxX = (rectA.maxX = center.x + 3);
			}
			for (int z = rectA.minZ; z <= rectA.maxZ; z++)
			{
				for (int x = rectA.minX; x <= rectA.maxX; x++)
				{
					yield return new IntVec3(x, 0, z);
				}
			}
			for (int z2 = rectB.minZ; z2 <= rectB.maxZ; z2++)
			{
				for (int x2 = rectB.minX; x2 <= rectB.maxX; x2++)
				{
					yield return new IntVec3(x2, 0, z2);
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <CalculateWindCells>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal CellRect <rectA>__0;

			internal CellRect <rectB>__0;

			internal int <offset>__0;

			internal Rot4 rot;

			internal int <neDist>__1;

			internal int <swDist>__1;

			internal IntVec3 center;

			internal int <z>__2;

			internal int <x>__3;

			internal int <z>__4;

			internal int <x>__5;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CalculateWindCells>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					rectA = default(CellRect);
					rectB = default(CellRect);
					offset = 0;
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
						rectB.minZ = (rectA.minZ = center.z - 3);
						rectB.maxZ = (rectA.maxZ = center.z + 3);
					}
					else
					{
						rectA.minZ = center.z + 2 + offset;
						rectA.maxZ = center.z + 2 + neDist + offset;
						rectB.minZ = center.z - 1 - swDist + offset;
						rectB.maxZ = center.z - 1 + offset;
						rectB.minX = (rectA.minX = center.x - 3);
						rectB.maxX = (rectA.maxX = center.x + 3);
					}
					z = rectA.minZ;
					goto IL_2F6;
				case 1u:
					x++;
					break;
				case 2u:
					x2++;
					goto IL_375;
				default:
					return false;
				}
				IL_2D1:
				if (x <= rectA.maxX)
				{
					this.$current = new IntVec3(x, 0, z);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				z++;
				IL_2F6:
				if (z > rectA.maxZ)
				{
					z2 = rectB.minZ;
					goto IL_39A;
				}
				x = rectA.minX;
				goto IL_2D1;
				IL_375:
				if (x2 <= rectB.maxX)
				{
					this.$current = new IntVec3(x2, 0, z2);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				z2++;
				IL_39A:
				if (z2 <= rectB.maxZ)
				{
					x2 = rectB.minX;
					goto IL_375;
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
				WindTurbineUtility.<CalculateWindCells>c__Iterator0 <CalculateWindCells>c__Iterator = new WindTurbineUtility.<CalculateWindCells>c__Iterator0();
				<CalculateWindCells>c__Iterator.rot = rot;
				<CalculateWindCells>c__Iterator.center = center;
				return <CalculateWindCells>c__Iterator;
			}
		}
	}
}
