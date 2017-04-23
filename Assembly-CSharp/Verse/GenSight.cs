using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect)
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
			IntVec3 c = default(IntVec3);
			while (i > 1)
			{
				c.x = num3;
				c.z = num4;
				if (endRect.Contains(c))
				{
					return true;
				}
				if (!startRect.Contains(c) && !c.CanBeSeenOverFast(map))
				{
					return false;
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

		[DebuggerHidden]
		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			GenSight.<PointsOnLineOfSight>c__Iterator245 <PointsOnLineOfSight>c__Iterator = new GenSight.<PointsOnLineOfSight>c__Iterator245();
			<PointsOnLineOfSight>c__Iterator.start = start;
			<PointsOnLineOfSight>c__Iterator.end = end;
			<PointsOnLineOfSight>c__Iterator.<$>start = start;
			<PointsOnLineOfSight>c__Iterator.<$>end = end;
			GenSight.<PointsOnLineOfSight>c__Iterator245 expr_23 = <PointsOnLineOfSight>c__Iterator;
			expr_23.$PC = -2;
			return expr_23;
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
	}
}
