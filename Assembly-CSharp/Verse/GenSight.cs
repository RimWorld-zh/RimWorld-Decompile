using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public static class GenSight
	{
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null, int halfXOffset = 0, int halfZOffset = 0)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x <= start.x) ? -1 : 1;
			int num6 = (end.z <= start.z) ? -1 : 1;
			num *= 4;
			num2 *= 4;
			num += halfXOffset * 2;
			num2 += halfZOffset * 2;
			int num7 = num / 2 - num2 / 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (!skipFirstCell || !(intVec == start))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			if (!start.InBounds(map) || !end.InBounds(map))
			{
				return false;
			}
			bool flag;
			if (start.x == end.x)
			{
				flag = (start.z < end.z);
			}
			else
			{
				flag = (start.x < end.x);
			}
			int num = Mathf.Abs(end.x - start.x);
			int num2 = Mathf.Abs(end.z - start.z);
			int num3 = start.x;
			int num4 = start.z;
			int i = 1 + num + num2;
			int num5 = (end.x <= start.x) ? -1 : 1;
			int num6 = (end.z <= start.z) ? -1 : 1;
			int num7 = num - num2;
			num *= 2;
			num2 *= 2;
			IntVec3 intVec = default(IntVec3);
			while (i > 1)
			{
				intVec.x = num3;
				intVec.z = num4;
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!intVec.CanBeSeenOverFast(map))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
				if (num7 > 0 || (num7 == 0 && flag))
				{
					num3 += num5;
					num7 -= num2;
				}
				else
				{
					num4 += num6;
					num7 += num;
				}
				i--;
			}
			return true;
		}

		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			bool sideOnEqual;
			if (start.x == end.x)
			{
				sideOnEqual = (start.z < end.z);
			}
			else
			{
				sideOnEqual = (start.x < end.x);
			}
			int dx = Mathf.Abs(end.x - start.x);
			int dz = Mathf.Abs(end.z - start.z);
			int x = start.x;
			int z = start.z;
			int i = 1 + dx + dz;
			int x_inc = (end.x <= start.x) ? -1 : 1;
			int z_inc = (end.z <= start.z) ? -1 : 1;
			int error = dx - dz;
			dx *= 2;
			dz *= 2;
			IntVec3 c = default(IntVec3);
			while (i > 1)
			{
				c.x = x;
				c.z = z;
				yield return c;
				if (error > 0 || (error == 0 && sideOnEqual))
				{
					x += x_inc;
					error -= dz;
				}
				else
				{
					z += z_inc;
					error += dx;
				}
				i--;
			}
			yield break;
		}

		public static bool LineOfSightToEdges(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
			{
				return true;
			}
			int num = (start * 2).DistanceToSquared(end * 2);
			for (int i = 0; i < 4; i++)
			{
				if ((start * 2).DistanceToSquared(end * 2 + GenAdj.CardinalDirections[i]) <= num)
				{
					if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, GenAdj.CardinalDirections[i].x, GenAdj.CardinalDirections[i].z))
					{
						return true;
					}
				}
			}
			return false;
		}

		[CompilerGenerated]
		private sealed class <PointsOnLineOfSight>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IntVec3 start;

			internal IntVec3 end;

			internal bool <sideOnEqual>__0;

			internal int <dx>__0;

			internal int <dz>__0;

			internal int <x>__0;

			internal int <z>__0;

			internal int <n>__0;

			internal int <x_inc>__0;

			internal int <z_inc>__0;

			internal int <error>__0;

			internal IntVec3 <c>__0;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PointsOnLineOfSight>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (start.x == end.x)
					{
						sideOnEqual = (start.z < end.z);
					}
					else
					{
						sideOnEqual = (start.x < end.x);
					}
					dx = Mathf.Abs(end.x - start.x);
					dz = Mathf.Abs(end.z - start.z);
					x = start.x;
					z = start.z;
					i = 1 + dx + dz;
					x_inc = ((end.x <= start.x) ? -1 : 1);
					z_inc = ((end.z <= start.z) ? -1 : 1);
					error = dx - dz;
					dx *= 2;
					dz *= 2;
					c = default(IntVec3);
					break;
				case 1u:
					if (error > 0 || (error == 0 && sideOnEqual))
					{
						x += x_inc;
						error -= dz;
					}
					else
					{
						z += z_inc;
						error += dx;
					}
					i--;
					break;
				default:
					return false;
				}
				if (i > 1)
				{
					c.x = x;
					c.z = z;
					this.$current = c;
					if (!this.$disposing)
					{
						this.$PC = 1;
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
				GenSight.<PointsOnLineOfSight>c__Iterator0 <PointsOnLineOfSight>c__Iterator = new GenSight.<PointsOnLineOfSight>c__Iterator0();
				<PointsOnLineOfSight>c__Iterator.start = start;
				<PointsOnLineOfSight>c__Iterator.end = end;
				return <PointsOnLineOfSight>c__Iterator;
			}
		}
	}
}
