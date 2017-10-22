using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class GenSight
	{
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null, int halfXOffset = 0, int halfZOffset = 0)
		{
			bool result;
			if (start.InBounds(map) && end.InBounds(map))
			{
				bool flag = (start.x != end.x) ? (start.x < end.x) : (start.z < end.z);
				int num = Mathf.Abs(end.x - start.x);
				int num2 = Mathf.Abs(end.z - start.z);
				int num3 = start.x;
				int num4 = start.z;
				int num5 = 1 + num + num2;
				int num6 = (end.x > start.x) ? 1 : (-1);
				int num7 = (end.z > start.z) ? 1 : (-1);
				num *= 4;
				num2 *= 4;
				num += halfXOffset * 2;
				num2 += halfZOffset * 2;
				int num8 = num / 2 - num2 / 2;
				IntVec3 intVec = default(IntVec3);
				while (num5 > 1)
				{
					intVec.x = num3;
					intVec.z = num4;
					if (!skipFirstCell || !(intVec == start))
					{
						if (!intVec.CanBeSeenOverFast(map))
							goto IL_0135;
						if ((object)validator != null && !validator(intVec))
							goto IL_0151;
					}
					if (num8 > 0 || (num8 == 0 && flag))
					{
						num3 += num6;
						num8 -= num2;
					}
					else
					{
						num4 += num7;
						num8 += num;
					}
					num5--;
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_01a7;
			IL_0135:
			result = false;
			goto IL_01a7;
			IL_0151:
			result = false;
			goto IL_01a7;
			IL_01a7:
			return result;
		}

		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			bool result;
			if (start.InBounds(map) && end.InBounds(map))
			{
				bool flag = (start.x != end.x) ? (start.x < end.x) : (start.z < end.z);
				int num = Mathf.Abs(end.x - start.x);
				int num2 = Mathf.Abs(end.z - start.z);
				int num3 = start.x;
				int num4 = start.z;
				int num5 = 1 + num + num2;
				int num6 = (end.x > start.x) ? 1 : (-1);
				int num7 = (end.z > start.z) ? 1 : (-1);
				int num8 = num - num2;
				num *= 2;
				num2 *= 2;
				IntVec3 intVec = default(IntVec3);
				while (num5 > 1)
				{
					intVec.x = num3;
					intVec.z = num4;
					if (endRect.Contains(intVec))
						goto IL_0110;
					if (!startRect.Contains(intVec))
					{
						if (!intVec.CanBeSeenOverFast(map))
							goto IL_0133;
						if ((object)validator != null && !validator(intVec))
							goto IL_014f;
					}
					if (num8 > 0 || (num8 == 0 && flag))
					{
						num3 += num6;
						num8 -= num2;
					}
					else
					{
						num4 += num7;
						num8 += num;
					}
					num5--;
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_01a5;
			IL_0133:
			result = false;
			goto IL_01a5;
			IL_014f:
			result = false;
			goto IL_01a5;
			IL_0110:
			result = true;
			goto IL_01a5;
			IL_01a5:
			return result;
		}

		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			if (start.x == end.x)
			{
				bool sideOnEqual = start.z < end.z;
			}
			else
			{
				bool sideOnEqual = start.x < end.x;
			}
			int dx2 = Mathf.Abs(end.x - start.x);
			int dz2 = Mathf.Abs(end.z - start.z);
			int x = start.x;
			int z = start.z;
			int i = 1 + dx2 + dz2;
			int x_inc = (end.x > start.x) ? 1 : (-1);
			int z_inc = (end.z > start.z) ? 1 : (-1);
			int error = dx2 - dz2;
			dx2 *= 2;
			dz2 *= 2;
			IntVec3 c = default(IntVec3);
			if (i > 1)
			{
				c.x = x;
				c.z = z;
				yield return c;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static bool LineOfSightToEdges(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			bool result;
			if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
			{
				result = true;
			}
			else
			{
				int num = (start * 2).DistanceToSquared(end * 2);
				for (int i = 0; i < 4; i++)
				{
					if ((start * 2).DistanceToSquared(end * 2 + GenAdj.CardinalDirections[i]) <= num && GenSight.LineOfSight(start, end, map, skipFirstCell, validator, GenAdj.CardinalDirections[i].x, GenAdj.CardinalDirections[i].z))
						goto IL_0099;
				}
				result = false;
			}
			goto IL_00b3;
			IL_00b3:
			return result;
			IL_0099:
			result = true;
			goto IL_00b3;
		}
	}
}
