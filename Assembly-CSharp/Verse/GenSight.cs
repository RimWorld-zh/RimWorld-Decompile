using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class GenSight
	{
		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, bool skipFirstCell = false, Func<IntVec3, bool> validator = null, int halfXOffset = 0, int halfZOffset = 0)
		{
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
						{
							return false;
						}
						if (validator != null && !validator(intVec))
						{
							return false;
						}
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
				return true;
			}
			return false;
		}

		public static bool LineOfSight(IntVec3 start, IntVec3 end, Map map, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
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
				return true;
			}
			return false;
		}

		public static IEnumerable<IntVec3> PointsOnLineOfSight(IntVec3 start, IntVec3 end)
		{
			if (start.x == end.x)
			{
				bool flag = start.z < end.z;
			}
			else
			{
				bool flag2 = start.x < end.x;
			}
			int dx = Mathf.Abs(end.x - start.x);
			int dz = Mathf.Abs(end.z - start.z);
			int x = start.x;
			int z = start.z;
			int i = 1 + dx + dz;
			if (end.x <= start.x)
				;
			if (end.z <= start.z)
				;
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
			if (GenSight.LineOfSight(start, end, map, skipFirstCell, validator, 0, 0))
			{
				return true;
			}
			int num = (start * 2).DistanceToSquared(end * 2);
			for (int i = 0; i < 4; i++)
			{
				if ((start * 2).DistanceToSquared(end * 2 + GenAdj.CardinalDirections[i]) <= num && GenSight.LineOfSight(start, end, map, skipFirstCell, validator, GenAdj.CardinalDirections[i].x, GenAdj.CardinalDirections[i].z))
				{
					return true;
				}
			}
			return false;
		}
	}
}
