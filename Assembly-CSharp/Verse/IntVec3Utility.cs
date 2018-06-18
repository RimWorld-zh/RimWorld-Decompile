using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EED RID: 3821
	public static class IntVec3Utility
	{
		// Token: 0x06005AF7 RID: 23287 RVA: 0x002E7188 File Offset: 0x002E5588
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x002E71A4 File Offset: 0x002E55A4
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x002E71C8 File Offset: 0x002E55C8
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x002E71EC File Offset: 0x002E55EC
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

		// Token: 0x06005AFB RID: 23291 RVA: 0x002E7290 File Offset: 0x002E5690
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x002E72D0 File Offset: 0x002E56D0
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06005AFD RID: 23293 RVA: 0x002E72F0 File Offset: 0x002E56F0
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			int a = Mathf.Min(v.x, v.z);
			a = Mathf.Min(a, map.Size.x - v.x - 1);
			a = Mathf.Min(a, map.Size.z - v.z - 1);
			return Mathf.Max(a, 0);
		}
	}
}
