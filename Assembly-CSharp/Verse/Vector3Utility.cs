using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EEF RID: 3823
	public static class Vector3Utility
	{
		// Token: 0x06005B00 RID: 23296 RVA: 0x002E7288 File Offset: 0x002E5688
		public static Vector3 HorizontalVectorFromAngle(float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x002E72B4 File Offset: 0x002E56B4
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

		// Token: 0x06005B02 RID: 23298 RVA: 0x002E730C File Offset: 0x002E570C
		public static Vector3 RandomHorizontalOffset(float maxDist)
		{
			float d = Rand.Range(0f, maxDist);
			float y = (float)Rand.Range(0, 360);
			return Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.forward * d;
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x002E7360 File Offset: 0x002E5760
		public static Vector3 RotatedBy(this Vector3 v3, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.up) * v3;
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x002E7388 File Offset: 0x002E5788
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

		// Token: 0x06005B05 RID: 23301 RVA: 0x002E742C File Offset: 0x002E582C
		public static float AngleToFlat(this Vector3 a, Vector3 b)
		{
			return new Vector2(a.x, a.z).AngleTo(new Vector2(b.x, b.z));
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x002E746C File Offset: 0x002E586C
		public static Vector3 FromAngleFlat(float angle)
		{
			Vector2 vector = Vector2Utility.FromAngle(angle);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x002E74A0 File Offset: 0x002E58A0
		public static float ToAngleFlat(this Vector3 v)
		{
			return new Vector2(v.x, v.z).ToAngle();
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x002E74D0 File Offset: 0x002E58D0
		public static Vector3 Abs(this Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}
	}
}
