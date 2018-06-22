using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EED RID: 3821
	public static class IntVec3Utility
	{
		// Token: 0x06005B1F RID: 23327 RVA: 0x002E91BC File Offset: 0x002E75BC
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x002E91D8 File Offset: 0x002E75D8
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x002E91FC File Offset: 0x002E75FC
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x002E9220 File Offset: 0x002E7620
		public static IntVec3 RotatedBy(this IntVec3 orig, Rot4 rot)
		{
			IntVec3 result;
			switch (rot.AsInt)
			{
			case 0:
				result = orig;
				break;
			case 1:
				result = new IntVec3(orig.z, orig.y, -orig.x);
				break;
			case 2:
				result = new IntVec3(-orig.x, orig.y, -orig.z);
				break;
			case 3:
				result = new IntVec3(-orig.z, orig.y, orig.x);
				break;
			default:
				result = orig;
				break;
			}
			return result;
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x002E92C4 File Offset: 0x002E76C4
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x002E9304 File Offset: 0x002E7704
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x002E9324 File Offset: 0x002E7724
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			int a = Mathf.Min(v.x, v.z);
			a = Mathf.Min(a, map.Size.x - v.x - 1);
			a = Mathf.Min(a, map.Size.z - v.z - 1);
			return Mathf.Max(a, 0);
		}
	}
}
