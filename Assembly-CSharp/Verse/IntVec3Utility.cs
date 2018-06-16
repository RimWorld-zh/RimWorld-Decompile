using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEE RID: 3822
	public static class IntVec3Utility
	{
		// Token: 0x06005AF9 RID: 23289 RVA: 0x002E70B0 File Offset: 0x002E54B0
		public static IntVec3 ToIntVec3(this Vector3 vect)
		{
			return new IntVec3(vect);
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x002E70CC File Offset: 0x002E54CC
		public static float DistanceTo(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontal;
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x002E70F0 File Offset: 0x002E54F0
		public static int DistanceToSquared(this IntVec3 a, IntVec3 b)
		{
			return (a - b).LengthHorizontalSquared;
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x002E7114 File Offset: 0x002E5514
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

		// Token: 0x06005AFD RID: 23293 RVA: 0x002E71B8 File Offset: 0x002E55B8
		public static int ManhattanDistanceFlat(IntVec3 a, IntVec3 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);
		}

		// Token: 0x06005AFE RID: 23294 RVA: 0x002E71F8 File Offset: 0x002E55F8
		public static IntVec3 RandomHorizontalOffset(float maxDist)
		{
			return Vector3Utility.RandomHorizontalOffset(maxDist).ToIntVec3();
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x002E7218 File Offset: 0x002E5618
		public static int DistanceToEdge(this IntVec3 v, Map map)
		{
			int a = Mathf.Min(v.x, v.z);
			a = Mathf.Min(a, map.Size.x - v.x - 1);
			a = Mathf.Min(a, map.Size.z - v.z - 1);
			return Mathf.Max(a, 0);
		}
	}
}
