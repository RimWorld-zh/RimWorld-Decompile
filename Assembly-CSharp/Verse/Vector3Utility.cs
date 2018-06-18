using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEE RID: 3822
	public static class Vector3Utility
	{
		// Token: 0x06005AFE RID: 23294 RVA: 0x002E7360 File Offset: 0x002E5760
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x002E738C File Offset: 0x002E578C
		public static float AngleFlat(this Vector3 v)
		{
			float result;
			if (v.x == 0f && v.z == 0f)
			{
				result = 0f;
			}
			else
			{
				result = Quaternion.LookRotation(v).eulerAngles.y;
			}
			return result;
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x002E73E4 File Offset: 0x002E57E4
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x002E7438 File Offset: 0x002E5838
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x002E7460 File Offset: 0x002E5860
		public static Vector3 RotatedBy(this Vector3 orig, Rot4 rot)
		{
			Vector3 result;
			switch (rot.AsInt)
			{
			case 0:
				result = orig;
				break;
			case 1:
				result = new Vector3(orig.z, orig.y, -orig.x);
				break;
			case 2:
				result = new Vector3(-orig.x, orig.y, -orig.z);
				break;
			case 3:
				result = new Vector3(-orig.z, orig.y, orig.x);
				break;
			default:
				result = orig;
				break;
			}
			return result;
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x002E7504 File Offset: 0x002E5904
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x002E7544 File Offset: 0x002E5944
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06005B05 RID: 23301 RVA: 0x002E7578 File Offset: 0x002E5978
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x002E75A8 File Offset: 0x002E59A8
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
