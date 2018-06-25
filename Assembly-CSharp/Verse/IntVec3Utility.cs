using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEF RID: 3823
	public static class IntVec3Utility
	{
		// Token: 0x06005B22 RID: 23330 RVA: 0x002E92DC File Offset: 0x002E76DC
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x002E92F8 File Offset: 0x002E76F8
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x002E931C File Offset: 0x002E771C
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x002E9340 File Offset: 0x002E7740
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

		// Token: 0x06005B26 RID: 23334 RVA: 0x002E93E4 File Offset: 0x002E77E4
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06005B27 RID: 23335 RVA: 0x002E9424 File Offset: 0x002E7824
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06005B28 RID: 23336 RVA: 0x002E9444 File Offset: 0x002E7844
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			int a = Mathf.Min(v.x, v.z);
			a = Mathf.Min(a, map.Size.x - v.x - 1);
			a = Mathf.Min(a, map.Size.z - v.z - 1);
			return Mathf.Max(a, 0);
		}
	}
}
